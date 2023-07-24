using System;
using UnityEngine;

// Cams mostly hack buoyancy
public class Buoyancy : MonoBehaviour
{
	public float splashVelocityThreshold;
	public float forceScalar;
	public float waterLineHack; // HACK

	public int underwaterVerts;
	public float dragScalar;

	public static event Action<GameObject, Vector3, Vector3> OnSplash;
	public static event Action<GameObject> OnDestroyed;

	Vector3 worldVertPos;

	private Rigidbody rb;
	private Mesh mesh;
	
	int meshNormalsAmount;
	private Vector3 myPos;
	private float meshVerticesLength;
	
	private Vector3[] meshNormals;
	private Vector3[] meshVerts;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		mesh = GetComponent<MeshFilter>().mesh;
		meshNormals = mesh.normals;
		meshVerts = mesh.vertices;
		
		meshNormalsAmount = meshNormals.Length;
		meshVerticesLength = meshNormals.Length;
	}

	void Update()
	{
		CalculateForces();
	}

	private void CalculateForces()
	{
		underwaterVerts = 0;
		float deltaTime = Time.deltaTime;
		myPos = transform.position;

		for (var index = 0; index < meshNormalsAmount; index++)
		{
			worldVertPos = myPos + transform.TransformDirection(meshVerts[index]);
			if (worldVertPos.y < waterLineHack)
			{
				// Splashes only on surface of water plane
				/*if (worldVertPos.y > waterLineHack - 0.1f)
				{
					float velocityMagnitude = rb.velocity.magnitude;
					if (velocityMagnitude > splashVelocityThreshold || rb.angularVelocity.magnitude > splashVelocityThreshold)
					{
						print(velocityMagnitude);
						if (OnSplash != null)
						{
							OnSplash.Invoke(gameObject, worldVertPos, rb.velocity);
						}
					}
				}*/
				rb.AddForceAtPosition(transform.TransformDirection(-meshNormals[index]) * (forceScalar * deltaTime), worldVertPos);
				underwaterVerts++;
			}
			// HACK to remove sunken boats
			if (worldVertPos.y < waterLineHack - 10f)
			{
				DestroyParentGO();
				break;
			}
			// Drag for percentage underwater
			rb.drag = underwaterVerts / meshVerticesLength * dragScalar;
			rb.angularDrag = underwaterVerts / meshVerticesLength * dragScalar;
		}
	}

	private void DestroyParentGO()
	{
		if (OnDestroyed != null)
		{
			OnDestroyed.Invoke(gameObject);
		}
		Destroy(gameObject);
	}
}

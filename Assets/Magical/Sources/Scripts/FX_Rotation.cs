using UnityEngine;
using System.Collections;
using DG.Tweening;
namespace MagicalFX
{
	public class FX_Rotation : MonoBehaviour
	{

		public Vector3 Speed = Vector3.up;

		void Start ()
		{
		}
	
		void FixedUpdate ()
		{
			this.transform.Rotate (Speed);
		}
        
	}
}
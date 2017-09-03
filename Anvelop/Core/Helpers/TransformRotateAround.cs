using UnityEngine;

namespace Anvelop.Core.Helpers
{
	public class TransformRotateAround
	{
		private readonly Transform _transform;
		private readonly float _angle;
		private readonly float _distance;

		public TransformRotateAround(Transform transform, float angle, float distance)
		{
			_transform = transform;
			_angle = angle;
			_distance = distance;
		}

		public TransformRotateAround Rotate()
		{
			_transform.localPosition = Vector3.zero;
			Vector3 radialPos = new Vector3(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad), 0);
			_transform.localPosition = radialPos * _distance;

			return this;
		}

		public TransformRotateAround AlignNormalTo(Transform to)
		{
			Vector3 normal = _transform.position - to.position;
			_transform.up = normal;

			return this;
		}

		public TransformRotateAround AlignNormalToZero()
		{
			Vector3 normal = _transform.localPosition - Vector3.zero;
			_transform.up = normal;

			return this;
		}
	}
}
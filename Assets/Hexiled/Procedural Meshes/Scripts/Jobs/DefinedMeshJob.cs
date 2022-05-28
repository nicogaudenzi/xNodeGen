using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Hexiled.ProceduralMeshes.Generators;
using Hexiled.ProceduralMeshes.Streams;

namespace Hexiled.ProceduralMeshes.Jobs
{

	[BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true, DisableSafetyChecks = true)]
	public struct DefinedMeshJob : IJobFor
	{
		SquareGrid generator;
		[WriteOnly]
		SingleStream streams;

		public void Execute(int j) => generator.Execute(j, streams);

		public static JobHandle ScheduleParallel(
			Mesh mesh, Mesh.MeshData meshData, int resolution, JobHandle dependency, NativeArray<TileDataStruct> data
		)
		{
			var job = new DefinedMeshJob();
			job.generator = new SquareGrid();
			job.streams = new SingleStream();

			job.generator.Resolution = resolution;
			job.generator.Data = data;
			job.streams.Setup(
				meshData, mesh.bounds = job.generator.Bounds, job.generator.VertexCount, job.generator.IndexCount
			);
			return job.ScheduleParallel(job.generator.JobLength, 1, dependency);
		}
	}

}
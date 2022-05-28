using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Hexiled.ProceduralMeshes.Generators;
using Hexiled.ProceduralMeshes.Streams;
namespace Hexiled.ProceduralMeshes
{
	[BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
	public struct MeshJob<G, S> : IJobFor
		where G : struct, IMeshGenerator
		where S : struct, IMeshStreams
	{
		G generator;
		[WriteOnly]
		S streams;

		public void Execute(int j) => generator.Execute(j, streams);

		public static JobHandle ScheduleParallel(
			Mesh mesh, Mesh.MeshData meshData,int resolution, JobHandle dependency
		)
		{
			var job = new MeshJob<G, S>();
			job.generator.Resolution = resolution;
			job.streams.Setup(
				meshData,mesh.bounds= job.generator.Bounds, job.generator.VertexCount, job.generator.IndexCount
			);
			return job.ScheduleParallel(job.generator.JobLength, 1, dependency);
		}

		public static JobHandle ScheduleParallel(
			Mesh mesh, Mesh.MeshData meshData, int resolution, JobHandle dependency, NativeArray<TileDataStruct> data
		)
		{
			var job = new MeshJob<G, S>();
			job.generator.Resolution = resolution;
			job.generator.Data = data;
			job.streams.Setup(
				meshData, mesh.bounds = job.generator.Bounds, job.generator.VertexCount, job.generator.IndexCount
			);
			return job.ScheduleParallel(job.generator.JobLength, 1, dependency);
		}
	}

}
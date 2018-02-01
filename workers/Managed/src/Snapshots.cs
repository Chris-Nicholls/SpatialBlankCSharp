using System;
using Improbable;
using Improbable.Collections;
using Improbable.Worker;
using Test.Component;

namespace Snapshots.snapshot
{
    class GenerateSnapshot
    {
        private static int size = 15;


        private static readonly WorkerRequirementSet TestWorker =
            new WorkerRequirementSet(new List<WorkerAttributeSet>
            {
                        new WorkerAttributeSet(new List<string> {"test"})
            });


        public static void Main(string[] args)
        {
            
            var filename = args[0];
            System.Console.Out.WriteLine($"Writing snapshot to {filename}");
            var output = new SnapshotOutputStream(filename);

            var id = 0;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {

                    output.WriteEntity(new EntityId(id++), CommanderEntity((x - size / 2) * 10, (y - size / 2) * 10));
                }
            }
            System.Console.Out.WriteLine("Finished writing snapshot");
            output.Dispose();

        }

        private static Entity CommanderEntity(int x, int y)
        {
            var aclData = new Map<uint, WorkerRequirementSet>();
            aclData.Add(Position.ComponentId, TestWorker);
            aclData.Add(Commander.ComponentId, TestWorker);
            aclData.Add(Metadata.ComponentId, TestWorker);

            var entity = BaseEntity(x, y, "test", aclData);
            entity.Add(new Commander.Data());

            return entity;
        }

        public static Entity BaseEntity(double x, double z, string name, Map<uint, WorkerRequirementSet> writeAcl)
        {
            var entity = new Entity();
            entity.Add(new Position.Data(new Coordinates(x, 0, z)));
            entity.Add(new Persistence.Data());
            entity.Add(new Metadata.Data(name));
            var aclData = new EntityAclData();
            aclData.readAcl = TestWorker;
            aclData.componentWriteAcl = writeAcl;
            entity.Add(new EntityAcl.Data(aclData));
            return entity;
        }
    }
}

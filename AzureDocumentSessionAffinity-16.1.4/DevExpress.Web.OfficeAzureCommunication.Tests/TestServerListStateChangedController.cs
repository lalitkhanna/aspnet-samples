#if DebugTest
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DevExpress.Web.OfficeAzureCommunication.Tests {

    [TestFixture]
    public class TestServerListStateChangedController {

        class StateContainer {
            public HashSet<string> State = null;
            public IEnumerable<string> ServerAdded = null;
            public IEnumerable<string> ServerRemoved = null;
            public ServerListStateChangedController ServerListStateChangedController;

            public StateContainer() {
                ServerListStateChangedController = new ServerListStateChangedController(
                    () => State,
                    (servers) => { ServerAdded = servers; },
                    (servers) => { ServerRemoved = servers; });
            }

            public void SetState(HashSet<string> state) {
                this.State = state;
            }
            public void BeginUpdate() {
                ServerListStateChangedController.BeginUpdate();
            }
            public void EndUpdate() {
                ServerListStateChangedController.EndUpdate();
            }
        }

        [Test]
        public void TestNoChanges() {
            var state = new StateContainer();
            HashSet<string> stateBeforeChanges = new HashSet<string>(new string[] { "Item1", "Item2" });
            HashSet<string> stateAfterChanges = new HashSet<string>(new string[] { "Item1", "Item2" });


            state.SetState(stateBeforeChanges);
            state.BeginUpdate();
            state.SetState(stateAfterChanges);
            state.EndUpdate();

            Assert.IsNull(state.ServerAdded);
            Assert.IsNull(state.ServerRemoved);
        }

        [Test]
        public void TestAdded() {
            var state = new StateContainer();
            string addedServer = "Item3";
            HashSet<string> stateBeforeChanges = new HashSet<string>(new string[] { "Item1", "Item2" });
            HashSet<string> stateAfterChanges = new HashSet<string>(new string[] { "Item1", "Item2", addedServer });


            state.SetState(stateBeforeChanges);
            state.BeginUpdate();
            state.SetState(stateAfterChanges);
            state.EndUpdate();

            Assert.AreEqual(1, state.ServerAdded.Count());
            Assert.AreEqual(addedServer, state.ServerAdded.First());
            Assert.IsNull(state.ServerRemoved);
        }

        [Test]
        public void TestRemoved() {
            var state = new StateContainer();
            string removerServer = "Item3";
            HashSet<string> stateBeforeChanges = new HashSet<string>(new string[] { "Item1", "Item2", removerServer });
            HashSet<string> stateAfterChanges = new HashSet<string>(new string[] { "Item1", "Item2"});


            state.SetState(stateBeforeChanges);
            state.BeginUpdate();
            state.SetState(stateAfterChanges);
            state.EndUpdate();

            Assert.IsNull(state.ServerAdded);
            Assert.AreEqual(1, state.ServerRemoved.Count());
            Assert.AreEqual(removerServer, state.ServerRemoved.First());
        }

        [Test]
        public void TestReplaced() {
            var state = new StateContainer();
            
            string addedServer1 = "Item5";
            string addedServer2 = "Item6";
            string removerServer1 = "Item3";
            string removerServer2 = "Item4";
            HashSet<string> stateBeforeChanges = new HashSet<string>(new string[] { "Item1", "Item2", removerServer1, removerServer2 });
            HashSet<string> stateAfterChanges = new HashSet<string>(new string[] { "Item1", "Item2", addedServer1, addedServer2 });

            state.SetState(stateBeforeChanges);
            state.BeginUpdate();
            state.SetState(stateAfterChanges);
            state.EndUpdate();

            Assert.AreEqual(2, state.ServerAdded.Count());
            Assert.IsTrue(state.ServerAdded.Contains(addedServer1));
            Assert.IsTrue(state.ServerAdded.Contains(addedServer2));
            Assert.AreEqual(2, state.ServerRemoved.Count());
            Assert.IsTrue(state.ServerRemoved.Contains(removerServer1));
            Assert.IsTrue(state.ServerRemoved.Contains(removerServer2));
        }
    }
}
#endif
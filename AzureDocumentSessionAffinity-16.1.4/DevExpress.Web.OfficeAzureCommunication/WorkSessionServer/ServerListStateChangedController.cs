using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpress.Web.OfficeAzureCommunication {

    class ServerListStateChangedController {
        StateChangedController stateChangedController;
        Action<IEnumerable<string>> serversAdded;
        Action<IEnumerable<string>> serversRemoved;

        public ServerListStateChangedController(
            Func<object> getStateDelegate,
            Action<IEnumerable<string>> serversAdded,
            Action<IEnumerable<string>> serversRemoved) {

            this.serversAdded = serversAdded;
            this.serversRemoved = serversRemoved;
            this.stateChangedController = new StateChangedController(getStateDelegate, checkStateChanged);
        }

        void checkStateChanged(object stateBeforeChanges, object stateAfterChanges) {
            var serversBeforeChanges = (IEnumerable<string>)stateBeforeChanges;
            var serversAfterChanges = (IEnumerable<string>)stateAfterChanges;

            var added = serversAfterChanges.Except(serversBeforeChanges);
            var removed = serversBeforeChanges.Except(serversAfterChanges);

            if(added.Count() > 0)
                this.serversAdded(added);

            if(removed.Count() > 0)
                this.serversRemoved(removed);

        }

        public void BeginUpdate() {
            stateChangedController.BeginUpdate();
        }
        public void EndUpdate() {
            stateChangedController.EndUpdate();
        }
    }

    class StateChangedController {
        object stateBeforeChanges = null;
        object stateAfterChanges = null;

        Func<object> getStateDelegate;
        Action<object, object> checkStateChanged;

        public StateChangedController(
            Func<object> getStateDelegate,
            Action<object, object> checkStateChanged) {

            this.getStateDelegate = getStateDelegate;
            this.checkStateChanged = checkStateChanged;
        }

        int counter = 0;
        public void BeginUpdate() {
            counter++;
            if(counter == 1)
                stateBeforeChanges = getStateDelegate();
        }
        public void EndUpdate() {
            counter--;

            if(counter == 0 && stateBeforeChanges != null) {
                stateAfterChanges = getStateDelegate();

                checkStateChanged(stateBeforeChanges, stateAfterChanges);
                stateBeforeChanges = null;
                stateAfterChanges = null;
            }

            if(counter < 0)
                counter = 0;
        }
    }

}

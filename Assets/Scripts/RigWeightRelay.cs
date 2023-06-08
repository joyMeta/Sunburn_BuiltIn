using Photon.Pun;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigWeightRelay : MonoBehaviour, IPunObservable {

    public Rig rig;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting)
            stream.SendNext(rig.weight);
        else
            rig.weight = (float) stream.ReceiveNext();
    }
}

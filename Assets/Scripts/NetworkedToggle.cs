using UnityEngine;
using UnityEngine.UI;
using UdonSharp;
using VRC.SDKBase;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class NetworkedToggle : UdonSharpBehaviour
{
    [SerializeField] protected Toggle m_toggle;

    [UdonSynced, FieldChangeCallback(nameof(SyncedToggleValue))]
    protected bool m_syncedToggleValue = false;
    protected bool SyncedToggleValue { set { m_syncedToggleValue = value; RequestSerialization(); } }

	public void OnToggleValueChanged(bool isOn)
	{
        BecomeOwnerIfNotAlready();
        SyncedToggleValue = m_toggle.isOn;
	}

	public override void OnDeserialization()
	{
        m_toggle.SetIsOnWithoutNotify(m_syncedToggleValue);
	}

	public void BecomeOwnerIfNotAlready()
    {
        if(!Networking.IsOwner(Networking.LocalPlayer, this.gameObject))
		{
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
		}
    }

}


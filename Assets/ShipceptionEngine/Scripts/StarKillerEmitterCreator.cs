using UnityEngine;
using System.Collections;
using VirtualDropkick.DanmakuEngine;
using VirtualDropkick.DanmakuEngine.Unity;


public class StarKillerEmitterCreator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private DanmakuContext context;

    void Awake()
    {
        context = DanmakuController.Instance.GetContext("MyContext");

        // register creation/destruction-delegates
        context.EmitterCreationHandler = CreateEmitter;
        context.EmitterDestructionHandler = DestroyEmitter;
    }

    private DanmakuEmitter CreateEmitter(EmitterModel emitterModel)
    {
        // put your construction logic here...
        BulletLibrary lib = context.bulletLibrary;
        DanmakuEmitter prefab = lib.defaultEmitter.prefab;
        DanmakuEmitter emitter = (Instantiate(prefab) as DanmakuEmitter);

        return emitter;
    }

    private void DestroyEmitter(DanmakuEmitter emitter)
    {
        // put your destruction logic here...
        Destroy(emitter.gameObject);
    }
}

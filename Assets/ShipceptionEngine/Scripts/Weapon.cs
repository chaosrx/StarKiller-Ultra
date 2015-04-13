using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using VirtualDropkick.DanmakuEngine;
using VirtualDropkick.DanmakuEngine.Unity;

public class Weapon : MonoBehaviour {

	public GameObject ProjectilePrefab;
	private DanmakuContext context;
    private DanmakuOrigin origin;
	public string contextName;
	public string varName;


    		private void Awake()
		{
            AwakeOrSpawned();
		}


	    private void OnSpawned()
	    {
	        AwakeOrSpawned();
	    }

    private void Start()
    {
        origin = GetComponent<DanmakuOrigin>();
    }

	void AwakeOrSpawned()
	{
		context = DanmakuController.Instance.GetContext(contextName);


		context.GetVariable(varName);


		// register creation/destruction-delegates
		context.BulletCreationHandler = CreateBullet;
		context.BulletDestructionHandler = DestroyBullet;
        context.EmitterCreationHandler = CreateEmitter;
        context.EmitterDestructionHandler = DestroyEmitter;
	}
	

	private DanmakuBullet CreateBullet(BulletModel bulletModel)
	{
		// Core GameKit Integration

		Transform bulletTransform = PoolBoss.Spawn(ProjectilePrefab.transform, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), PoolBoss.Instance.transform);
		DanmakuBullet bullet = bulletTransform.GetComponentInChildren<DanmakuBullet>();

		return bullet;
	}

    public void Fire(List<KeyCode> fireKeyCodes)
    {
        foreach (KeyCode element in fireKeyCodes)
        {
            if (Input.GetKey(element))
            {

                if (!origin.IsRunningBulletPattern)
                {
                    origin.StartBulletPattern();
                }

                if (!origin.RootEmitter.enabled)
                {
                    origin.RootEmitter.enabled = true;
                }



            }
            else if (origin.IsRunningBulletPattern && origin.RootEmitter.enabled)
            {
                origin.RootEmitter.enabled = false;
                origin.ResetRootEmitter();
            }

        } 
    }

	private void DestroyBullet(DanmakuBullet bullet)
	{
		//Core GameKit Integration
		PoolBoss.Despawn(bullet.transform);
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

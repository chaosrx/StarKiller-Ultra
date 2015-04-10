using UnityEngine;
using System.Collections;
using VirtualDropkick.DanmakuEngine;
using VirtualDropkick.DanmakuEngine.Unity;

public class StarKillerBulletCreator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private DanmakuContext context;

    void Awake()
    {
        AwakeOrSpawned();
    }

    void OnSpawned()
    {
        
        AwakeOrSpawned();
    }

    void AwakeOrSpawned()
    {
        context = DanmakuController.Instance.GetContext("PlayerContext");

        // register creation/destruction-delegates
        context.BulletCreationHandler = CreateBullet;
        context.BulletDestructionHandler = DestroyBullet;
    }

    private DanmakuBullet CreateBullet(BulletModel bulletModel)
    {
        // put your construction logic here...
        BulletLibrary lib = context.bulletLibrary;
        DanmakuBullet prefab = lib.GetBulletPrefab(bulletModel.Id);
        DanmakuBullet bullet = (Instantiate(prefab) as DanmakuBullet);

        return bullet;
    }

    private void DestroyBullet(DanmakuBullet bullet)
    {
        // put your destruction logic here...
        Destroy(bullet.gameObject);
    }
}

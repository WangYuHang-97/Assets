using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class AssetBundleTest : MonoBehaviour
{

    void Awake()
    {
        // 全局只需初始化一次
        ResMgr.Init();
    }

    /// <summary>
    /// 每一个需要加载资源的单元（脚本、界面）申请一个 ResLoader
    /// ResLoader 本身会记录该脚本加载过的资源
    /// </summary>
    /// <returns></returns>
    ResLoader mResLoader = ResLoader.Allocate();

    void Start()
    {
        LoadPrefab();
        LoadSprite();
    }

    /// <summary>
    /// 加载预制体
    /// </summary>
    void LoadPrefab()
    {
        // 通过 LoadSync 同步加载资源
        // 只需要传入资源名即可，不需要传入 AssetBundle 名。
        //mResLoader.LoadSync<GameObject>("AssetBundleTest1")
        //    .Instantiate();

        // 通过 LoadAsync 异步加载资源
        // 添加到加载队列
        // 添加一个资源
        mResLoader.Add2Load("AssetBundleTest1", (succeed, res) => {
            if (succeed)
            {
                res.Asset.As<GameObject>()
                    .Instantiate();
            }
        });

        // 加载一个资源,不处理
        mResLoader.Add2Load("AssetBundleTest1", (succeed, res) => { });

        // 加载一个列表中的资源
        mResLoader.Add2Load(new List<string>() { "AssetBundleTest1", "AssetBundleTest2" });

        // 执行加载操作
        mResLoader.LoadAsync(() => {
            // 可以监听所有的资源是否加载成功
            "资源加载成功".LogInfo();
        });
    }

    /// <summary>
    /// 模拟模式下加载图片
    /// 非模拟模式下不同，具体参考https://qframework.cn/doc/detail/ae9dd030-a783-450c-9d03-e77c5e231389
    /// </summary>
    void LoadSprite()
    {
        //打包加载资源
        var sprite = mResLoader.LoadSprite(QAssetBundle.Loadsprite_jpg.LOADSPRITE);//需要在Res Kit中生成代码（资源名常量）
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        //resource下加载资源
        //var sprite = mResLoader.LoadSprite("resources://LoadSprite");
        //var spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite = sprite;
    }

    void OnDestroy()
    {
        // 释放所有本脚本加载过的资源
        // 释放只是释放资源的引用
        // 当资源的引用数量为 0 时，会进行真正的资源卸载操作
        mResLoader.Recycle2Cache();
        mResLoader = null;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Gender
{
    /// <summary>
    /// 男
    /// </summary>
    male,
    /// <summary>
    /// 女
    /// </summary>
    female
}

/// <summary>
/// 受伤的身体部位
/// </summary>
public enum InjuredBodyPart
{
    /// <summary>
    /// 手受伤 攻击力下降
    /// </summary>
    hand,
    /// <summary>
    /// 躯干 睡觉，治疗时的回复力下降
    /// </summary>
    trunk,
    /// <summary>
    /// 攻击的命中率下降
    /// </summary>
    head,
    /// <summary>
    /// 移动，探索时耐力消耗增加
    /// </summary>
    leg,
    /// <summary>
    /// 中毒  移动、探索时生命减少。
    /// </summary>
    poisoning,
    None
}

public enum EquipSlot
{
    weapon,
    /// <summary>
    /// 弹药
    /// </summary>
    cartridge,
    head,
    /// <summary>
    /// 躯干
    /// </summary>
    trunk,
    hand,
    feet,
    /// <summary>
    /// 项链,
    /// </summary>
    necklace,
    /// <summary>
    /// 面具,
    /// </summary>
    faceMask,
    /// <summary>
    /// 内衣,
    /// </summary>
    underwear,
    /// <summary>
    /// 饰品
    /// </summary>
    accessory,
}


/// <summary>
/// 基础姿态
/// </summary>
public enum BasePose
{
    /// <summary>
    /// 普通
    /// </summary>
    common,
    /// <summary>
    /// 攻击
    /// </summary>
    attack,
    /// <summary>
    /// 防御
    /// </summary>
    defend,
    /// <summary>
    /// 隐藏
    /// </summary>
    hide,
    /// <summary>
    /// 探索
    /// </summary>
    discover,
    /// <summary>
    /// 治疗,恢复
    /// </summary>
    cure
}

/// <summary>
/// 战斗策略
/// </summary>
public enum ButtleStrategy
{
    common,
    /// <summary>
    /// 重视回击，重视反击
    /// </summary>
    emphasizeFightBack,
    /// <summary>
    /// 重视防御
    /// </summary>
    emphasizeDefent,
    /// <summary>
    /// 重视躲闪/躲避
    /// </summary>
    emphasizeDodge,
}

public enum SchoolClub
{
    /// <summary>
    /// 棒球社 殴 初始熟练20
    /// </summary>
    Bang4Qiu2,
    /// <summary>
    /// 击剑社 斩 初始熟练20
    /// </summary>
    Ji1Jian4,
    /// <summary>
    /// 弓道社 斩 初始熟练20
    /// </summary>
    Gong,
    /// <summary>
    /// 篮球社 投 初始熟练20	
    /// </summary>
    Lang2Qiu2,
    /// <summary>
    /// 化学社 爆 初始熟练20 合成道具时有额外爆熟加成
    /// </summary>
    Hua4Xue2,

    /// <summary>
    /// 足球社 移动时消耗的体力减少
    /// </summary>
    Zu2Qiu2,
    /// <summary>
    /// 侦探社 搜索时消耗的体力减少
    /// </summary>
    Zhen1Tan4,
    /// <summary>
    /// 电脑社 hack成功率100%
    /// </summary>
    Dian4Nao3,
    /// <summary>
    /// 动漫社 可自由控制怒气使用必杀技
    /// </summary>
    Dong4Mang4,
    /// <summary>
    /// 烹饪社 可以使用“检查毒物”指令。下毒效果加倍。合成食物有数量加成。
    /// </summary>
    Peng1Ren4,
    /// <summary>
    /// 柔道社 殴 初始熟练20  投 初始熟练20
    /// </summary>
    Rou2Dao4,
    /// <summary>
    /// 医学社 可以免体力包扎伤口。 使用补给品时，会有1.2倍效果。 中毒扣除毒物效果一半生命，中毒掉血减半。
    /// </summary>
    Yi1Xue2,
    /// <summary>
    /// 发明社 可以花费一定体力进行武器精炼。武器精炼能够牺牲武器耐久，提高武器效果。
    /// </summary>
    Fa1Ming2,
    /// <summary>
    /// 魔术社 合成物品时(特殊除外)，会有1.1倍效果。使用针线包，效果会增幅，使用磨刀石等物品，成功率提高。
    /// </summary>
    Mo2Shu4,
    /// <summary>
    /// 经济社 购买物品享受85折。
    /// </summary>
    Jin1Ji4,

}

public class ButtleProperty
{
    /// <summary>
    /// 移动速度,根据场景间的距离来计算所需要的时间,移动速度越快,所需时间越少
    /// </summary>
    public int baseSpeed = 20;
    public int moveSpeed = 20;
    /// <summary>
    ///  天生属性之一,美丽度最低为0,表示无比丑陋,最高为100表示非常美丽,游戏中可以根据穿着来调节美丽度的值
    ///  美丽度达到90以上可以使用以美有关的技能,如魅惑,降服
    /// </summary>
    public int beautifulness;
    /// <summary>
    /// 智商,平均智商为100,天才为140以上,弱智为60以下,当智商达到110以上人物会拥有智力方面的天赋技能,如金蝉脱壳
    /// </summary>
    public int iq;

    #region 绝境

    public int attack;
    public int baseAttack = 100;
    public int defence;
    public int baseDefence = 100;

    /// <summary>
    /// 体力,耐力
    /// </summary>
    public int stamina;
    public int maxStamina = 1000;
    public int health;
    public int maxHealth = 1000;
    /// <summary>
    /// 口渴度 0为不渴,最大为1000
    /// </summary>
    public int thirsty;
    public int initialThirsty = 1000;
    /// <summary>
    /// 饥饿度 0为不饿,最大为1000
    /// </summary>
    public int hunger;
    public int initialHunger = 1000;
    /// <summary>
    /// 力量
    /// </summary>
    public float strength;
    public float baseStrength = 20;
    //public float maxStrength;
    /// <summary>
    /// 精准
    /// </summary>
    public float accurate;
    public float baseAccurate = 20;
    /// <summary>
    /// 体质
    /// </summary>
    public float physique;
    public float basePhysique = 20;
    /// <summary>
    /// 幸运
    /// </summary>
    public float luck;
    public float baseLuck = 20;
    /// <summary>
    /// 探索
    /// </summary>
    public float exploration;
    public float baseExploration = 20;
    /// <summary>
    /// 最大携带重量
    /// </summary>
    public int maxCarryWeight;
    public int initialMaxCarryWeight = 2000;
    /// <summary>
    /// 包裹体积
    /// </summary>
    public float packVolume;
    public float initialPackVolume = 5;

    #endregion 绝境

    #region BR Property

    /// <summary>
    /// 怒气
    /// </summary>
    public int anger;
    public int maxAnger = 100;
    /// <summary>
    /// 基础姿态
    /// </summary>
    public BasePose basePose = BasePose.common;
    /// <summary>
    /// 战斗策略
    /// </summary>
    public ButtleStrategy buttleStrategy = ButtleStrategy.common;

    /// <summary>
    /// 学校社团
    /// </summary>
    public SchoolClub schoolClub;

    /// <summary>
    /// 欧系熟练度
    /// </summary>
    public int beatupProficiency = 0;
    /// <summary>
    /// 斩系熟练度
    /// </summary>
    public int cutProficiency = 0;
    /// <summary>
    /// 射系熟练度
    /// </summary>
    public int shootProficiency = 0;
    /// <summary>
    /// 投系熟练度
    /// </summary>
    public int throwProficiency = 0;
    /// <summary>
    /// 爆系熟练度
    /// </summary>
    public int explosiveProficiency = 0;

    #endregion BR Property
}


public class PlayerCharacter : MonoBehaviour,IPicture {
    ///// <summary>
    ///// SubServer上使用的识别id 默认值是0
    ///// </summary>
    //public int netID { get { return _netID; } set { _netID = value; Debug.Log("netID = " + value); } }
    //[SerializeField]
    //private int _netID = 0;

    //public NetworkPlayer networkPlayer;
    public Player player;
    public string characterName;
    public Gender gender;
    public int studentID;
    public Team team;
    public int grade = 1;
    public int exp = 0;
    /// <summary>
    /// 受伤部位
    /// </summary>
    public List<InjuredBodyPart> injuredBodyPartList;
    public int money;
    public int killCount;
    public List<BrObject> package = new List<BrObject>(12);
    public ButtleProperty buttleProp;

    public Texture FindPictrue(int id)
    {
        throw new System.NotImplementedException();
    }



}

using UnityEngine;
using System.Collections;


/// <summary>
/// 武器
/// [性别加成]	有些武器有特殊性别加成，打同性或异性时可能产生伤害只有 1 或大于100的爆击效果。 
///所谓性别加成是看双方的性别， 不是只看使用者的性别。 
///此类武器不会特别显示， 请玩家自行观察注意。
/// </summary>
public class Weapon : Equipment {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
/// <summary>
/// 爆系武器特点
/// 溅射	有几率对当前地图一定数量敌人造成溅射伤害。(伤害均为爆系武器效果值)
/// ITD 保证伤害值，让低等级也能挑战高等级	
/// 爆系武器数量少
/// </summary>
public class ExplosiveWeapon : Weapon
{

}

/// <summary>
/// 射击系武器特点
/// 破甲术	有几率穿透对手的护甲。有几率令敌人的护甲效果值下降。
/// 初期高攻击力	
/// 要上子弹
/// </summary>
public class ShootWeapon : Weapon
{

}

/// <summary>
/// 投系武器特点
/// 精准打击	有几率令对手造成负面状态"虚弱"(可治疗)。当投系攻击具有"虚弱"状态的敌人时，即可造成重击。
/// 命中率高，最适合磨掉装甲	
/// 攻击力小
/// </summary>
public class ThrowWeapon : Weapon
{

}

/// <summary>
/// 斩系武器特点
/// 二刀流	有几率在一次作战中对对手造成2次攻击(增加2点熟练)。
/// 可变动攻击力，让对手负伤范围最广	
/// 变动攻击力,最高损坏率
/// </summary>
public class CutWeapon : Weapon
{

}

/// <summary>
/// 欧系武器特点
/// 格挡	有几率按照百分比吸收对手的伤害。按照手中欧系武器效果强弱，增加一定量防御力。
/// 流星拳	有几率在一次作战中对对手造成2连击(增加2点熟练)。
/// 空手也有加成,少数有变动攻击力	
/// 原始攻击力普通
/// </summary>
public class BeatupWeapon : Weapon
{

}
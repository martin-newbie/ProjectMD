

public interface IKeyword
{
    KeywordType id { get; }
    int count { get; set; }
}

public interface IKeywordOnFrame
{
    void OnFrame(UnitBehaviour target);
}

public interface IKeywordOnSecond
{
    void OnSecond(UnitBehaviour target);
}

public interface IKeywordOnDamage
{
    void OnDamage(DamageStruct damage, Entity from, Entity target);
}

public enum KeywordType
{
    TREMOR,
    SHOCK, // 감전
    BLEED,
    BURN,
}


public interface IKeyword
{
    string id { get; }
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
    void OnDamage(DamageStruct damage, UnitBehaviour from, UnitBehaviour target);
}

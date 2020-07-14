public class SubScreenBase
{
    // 关联SubCtrl
    protected SubUICtrlBase mCtrlBase;

    public SubUICtrlBase CtrlBase { get { return mCtrlBase; } }

    // 利用SubCtrl来进行初始化
    public SubScreenBase(SubUICtrlBase ctrlBase)
    {
        mCtrlBase = ctrlBase;
        Init();
    }

    virtual protected void Init()
    {

    }

    virtual public void Dispose()
    {

    }
}

namespace Ileri_Programlama.Models;

public enum EmailChallengePurpose
{
    Checkout,
    Login,
    Reset
}

public enum SessionStatus
{
    Pending,
    Active,
    Completed
}

public enum SessionType
{
    Puanli,
    Alistirma
}

public enum PuanlamaTuru
{
    Standart,
    ZamanaGore,
    CezaPuanli
}
public class ProfilViewModel
{
    public string FullName { get; set; } = string.Empty;

    // Malá fotka (avatar)
    public string? ProfilePicturePath { get; set; }

    // Velká fotka (profilová)
    public string? ProfilePictureLargePath { get; set; }
}
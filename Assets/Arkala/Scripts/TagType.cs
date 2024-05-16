namespace ClickNext.Scripts
{
    public enum TagType: int
    {
        Untagged,
        Materials,
        Storage,
        Animal,
        Tree,
        Rock,
        Building,
        Farm,
        Grass
    }

    public enum LayerType: int
    {
        AllLayers = -1,
        Blueprint = 6
    }

    public enum MaterialType: int
    {
        Meat,
        Wood,
        Stone,
        Grain,
        Flour,
        Food
    }
}

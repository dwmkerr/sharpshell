namespace SharpShell.Interop
{
    public enum FontClipPrecision : byte
    {
        CLIP_DEFAULT_PRECIS = 0,
        CLIP_CHARACTER_PRECIS = 1,
        CLIP_STROKE_PRECIS = 2,
        CLIP_MASK = 0xf,
        CLIP_LH_ANGLES = (1 << 4),
        CLIP_TT_ALWAYS = (2 << 4),
        CLIP_DFA_DISABLE = (4 << 4),
        CLIP_EMBEDDED = (8 << 4),
    }
}
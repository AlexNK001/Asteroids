public sealed class FieldBoundaryConfig
{
    public FieldBoundaryConfig(float top, float bottom, float right, float left)
    {
        Top = top;
        Bottom = bottom;
        Right = right;
        Left = left;
    }

    public float Top { get;  }
    public float Bottom { get;  }
    public float Right { get;  }
    public float Left { get;  }
}
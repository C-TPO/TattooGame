using System;
using UnityEngine;

[Serializable]
public struct TattooScoreResult
{
    [Range(0f, 100f)] public float totalScore;
    [Range(0f, 100f)] public float lineCoverage;
    [Range(0f, 100f)] public float linePrecision;

    public TattooScoreResult(float totalScore, float lineCoverage, float linePrecision)
    {
        this.totalScore = totalScore;
        this.lineCoverage = lineCoverage;
        this.linePrecision = linePrecision;
    }
}

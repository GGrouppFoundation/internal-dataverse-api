using System;

namespace GarageGroup.Infra;

public sealed record class DataverseExpandedField
{
    public DataverseExpandedField(string fieldName, FlatArray<string> selectFields, FlatArray<DataverseExpandedField> expandFields)
    {
        FieldName = fieldName ?? string.Empty;
        SelectFields = selectFields;
        ExpandFields = expandFields;
    }

    public DataverseExpandedField(string fieldName, FlatArray<string> selectFields)
    {
        FieldName = fieldName ?? string.Empty;
        SelectFields = selectFields;
    }

    public DataverseExpandedField(string fieldName)
        =>
        FieldName = fieldName ?? string.Empty;

    public string FieldName { get; }

    public FlatArray<string> SelectFields { get; init; }

    public FlatArray<DataverseExpandedField> ExpandFields { get; init; }
}
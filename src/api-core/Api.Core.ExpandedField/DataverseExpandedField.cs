using System;

namespace GGroupp.Infra;

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

    public string FieldName { get; }

    public FlatArray<string> SelectFields { get; }

    public FlatArray<DataverseExpandedField> ExpandFields { get; }
}
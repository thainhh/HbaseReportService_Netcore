//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: CellMessage.proto
namespace HbaseReportService.Hbase.Generated
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Cell")]
  public partial class Cell : global::ProtoBuf.IExtensible
  {
    public Cell() {}
    
    private byte[] _row = null;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"row", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public byte[] row
    {
      get { return _row; }
      set { _row = value; }
    }
    private byte[] _column = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"column", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public byte[] column
    {
      get { return _column; }
      set { _column = value; }
    }
    private long _timestamp = default(long);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"timestamp", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(long))]
    public long timestamp
    {
      get { return _timestamp; }
      set { _timestamp = value; }
    }
    private byte[] _data = null;
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"data", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public byte[] data
    {
      get { return _data; }
      set { _data = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}
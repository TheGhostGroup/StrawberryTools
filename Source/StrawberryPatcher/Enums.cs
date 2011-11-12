// BytesCount not used...
enum BytesCount : int
{
    PATCHED_BYTES_COUNT = 5
}

enum Offsets : int
{
    Send2Offset        = 0x8705A,
    CommsHandlerOffset = 0x86685,
    versionOffset      = 0x218CE0    // Bytes: 90 30
}

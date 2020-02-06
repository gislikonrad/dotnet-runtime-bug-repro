# Reproduction of dotnet runtime issue

This is a repo to reproduce issue [31862](https://github.com/dotnet/runtime/issues/31862) in the dotnet/runtime repo. 

## Reproduction environment

If this matters at all

* Microsoft Windows 10 Enterprise
* Visual Studio 2019 Professional 16.4.4 debugger
* Both projects run simultaniously
* Both projects run directly (not iisexpress)
* Request GET - http://localhost:5000
* View both consoles and see the discrepency

### Example
(line breaks added for readability)

### From parent log

RequestPath:/ 
RequestId:0HLTB4M854SQ4:00000002, 
SpanId:1ba1509359745746, 
TraceId:3f2eb7591880f8479ace2a3dd3e8d2f3, 
ParentId:0000000000000000

### From child log

RequestPath:/ 
RequestId:0HLTB4M84SAFF:00000002, 
SpanId:854f03350d0cfb4e, 
TraceId:3f2eb7591880f8479ace2a3dd3e8d2f3, 
ParentId:28ba1d36d1be4546

## Hack

Lines 27-30 in the Startup class of the Tracing project include commented out code that mitigates this issue using reflection.

### Example (with hack)
(line breaks added for readability)

#### From parent log

RequestPath:/ 
RequestId:0HLTB4L5AQAU1:00000002, 
SpanId:87b86853f9c2c143, 
TraceId:732b8f1ee37a384e895bb5c3ec736cdc, 
ParentId:0000000000000000

#### From child log

RequestPath:/ 
RequestId:0HLTB4L5ADT20:00000002, 
SpanId:075fd8546433184f, 
TraceId:732b8f1ee37a384e895bb5c3ec736cdc, 
ParentId:87b86853f9c2c143
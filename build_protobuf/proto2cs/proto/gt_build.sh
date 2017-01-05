#!/bin/bash
#protoc -I=. --cpp_out=/Users/tinyult/Development/coopserver/protobuf_files/cpp_out/. ./*.proto
director=~+
protoc -I=. --csharp_out=/Users/tinyult/Documents/Code/GameCenter3.0/Assets/Scripts/NetWork/Proto ./*.proto

#protoc -I=. --plugin=protoc-gen-go=/Users/tinyult/Development/coopserver/protobuf_files/protoc-gen-go --go_out=/Users/tinyult/Development/coopserver/protobuf_files/go_out ./*.proto

cmake_minimum_required(VERSION 3.12)

# Project
project(DllExportScanner.Tests.Native VERSION 0.0.1 LANGUAGES CXX)
add_library(DllExportScanner.Tests.Native SHARED)
target_compile_features(DllExportScanner.Tests.Native PUBLIC cxx_std_20)

# Defines
target_compile_definitions(DllExportScanner.Tests.Native
	PRIVATE NATIVE_EXPORTS
)

# Source files
target_sources(DllExportScanner.Tests.Native
	PRIVATE	"Native.cpp"
)

# Install Files 
install(TARGETS DllExportScanner.Tests.Native
    LIBRARY DESTINATION "lib"
    ARCHIVE DESTINATION "lib"
    RUNTIME DESTINATION "bin"
)

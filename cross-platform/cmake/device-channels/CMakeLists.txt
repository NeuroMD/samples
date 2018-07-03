cmake_minimum_required(VERSION 3.3 FATAL_ERROR)

# project name and supported languages
project(device-channels CXX)

# detect operating system and host processor
message(STATUS "We are on a ${CMAKE_SYSTEM_NAME} system")
message(STATUS "The host processor is ${CMAKE_HOST_SYSTEM_PROCESSOR}")

link_directories($ENV{NEUROSDK})
include_directories($ENV{NEUROSDK}/include)
message(STATUS $ENV{NEUROSDK})

set(CMAKE_EXE_LINKER_FLAGS "${CMAKE_EXE_LINKER_FLAGS}")
set(CMAKE_CXX_STANDARD_LIBRARIES "${CMAKE_EXE_LINKER_FLAGS}")
set(CMAKE_CONFIGURATION_TYPES "Debug;Release;")
set(CMAKE_CXX_FLAGS "-std=c++14")

add_executable (device-channels main.cpp)
target_link_libraries(device-channels debug neurosdkd)
target_link_libraries(device-channels optimized neurosdk)

add_custom_command(TARGET device-channels POST_BUILD        
    COMMAND ${CMAKE_COMMAND} -E copy_if_different  
        "$ENV{NEUROSDK}/neurosdkd.dll"      
        $<TARGET_FILE_DIR:device-channels>)   
cmake_minimum_required(VERSION 3.3 FATAL_ERROR)

# project name and supported languages
project(device-info CXX)

# detect operating system and host processor
message(STATUS "We are on a ${CMAKE_SYSTEM_NAME} system")
message(STATUS "The host processor is ${CMAKE_HOST_SYSTEM_PROCESSOR}")

link_directories($ENV{NEUROSDK})
include_directories($ENV{NEUROSDK}/include)
message($ENV{NEUROSDK})

add_executable (device-info main.cpp)

target_link_libraries(device-info neurosdk ws2_32)

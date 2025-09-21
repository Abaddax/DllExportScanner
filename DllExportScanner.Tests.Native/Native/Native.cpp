//Windows
#ifdef _WIN32
#ifdef NATIVE_EXPORTS
#define NATIVE_API __declspec(dllexport)
#else
#define NATIVE_API __declspec(dllimport)
#endif
//Linux
#else
#define NATIVE_API __attribute__((visibility("default")))
#endif


extern "C"
{
	int NATIVE_API Test_Export01(int a, int b)
	{
		return a + b;
	}
}

namespace TEST_CPP_NAMESPACE
{
	int NATIVE_API Test_Export02(int a, int b)
	{
		return a + b;
	}
	int NATIVE_API Test_Export02(int a, int b, int c)
	{
		return a + b + c;
	}
}

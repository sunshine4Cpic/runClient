@echo off 
@echo ��ʼִ��
adb -s %3 shell mkdir /sdcard/test 
adb -s %3 shell rm /sdcard/test/*.* 
adb -s %3 push %1 /sdcard/test/test.xml
if not "%5"=="no" (
	adb -s %3 shell pm clear %5
)
if  not "%4"=="no" (
	adb -s %3 uninstall com.WebControl.control 
	adb -s %3 install %4 
)
adb -s %3 shell am instrument -w com.WebControl.control/android.test.InstrumentationTestRunner 
adb -s %3 pull /sdcard/test/ %2 
@echo ִ�����

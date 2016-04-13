@echo off 
@echo 开始执行
adb -s %3 shell pm clear %5 >>%2log.txt 2>&1
adb -s %3 uninstall com.WebControl.control >>%2log.txt 2>&1
adb -s %3 install %4 >>%2log.txt 2>&1
adb -s %3 shell mkdir /sdcard/test >>%2log.txt 2>&1
adb -s %3 shell rm /sdcard/test/*.* >>%2log.txt 2>&1
adb -s %3 push %1 /sdcard/test/test.xml >>%2log.txt 2>&1
adb -s %3 shell am instrument -w com.WebControl.control/android.test.InstrumentationTestRunner >>%2log.txt 2>&1
adb -s %3 pull /sdcard/test/ %2 >>%2log.txt 2>&1
@echo 执行完成

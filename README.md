# procopac

A command line tool to change a window's opacity on Windows.

## Usage

procopac [procname/PID] [opacity]

- [procname/PID] should be the executable name without the '.exe' extension or the PID
- [opacity] should be an integer number between 40 and 255

## Notes

Only tested on Windows 10 but since it uses WinAPI it *should* be backwards compatible.

Inspired by: [window-opacity ](https://github.com/DongchengWang/window-opacity)

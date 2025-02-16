#include <windows.h>
#include <gdiplus.h>
#include <ctime>
#include <cstdio>

int wx = 300;
int wy = 300;

int modehandler = 0;
int maxframes = 160;
char buffer[1024] = {0};
int idx = 0;
int typedmax = 0;
int scrollx = 699;
int scrollsize,windowidth;
wchar_t filefromframe[256];
int frame = 0;
HWND hwndTaskbar = FindWindow("Shell_TrayWnd", NULL);

const char* rngtext[] = {
    "Este programa foi criado pelo saladino, enquanto isso pesquise e621 e aprecie como a internet é hoje em dia.",
                "Ele queima ou não queima?? VISHHHHH DAYM",
                "Aí o que eu disse pra ele eu falei OiIiIiiIi EU Tenho fetiche de gigantismo e falei pra ele a vai tomar no cu fdp",
                "Roblox vai acabar em 2030 ve o que estou falando,aí ninguem vai ter robux roblox roblox"
                , "Meu mano foi num lugar que ele morava antes sabe aí quando a casa dele pegou fogo falaram logo que ele era chris chan"
                , "As disciplinas da humanidades são quando um ser humano começa quer uma atração estranha com tubarões gays e especificamente com olhos FUCK ME.",
                "EU SOU DJ DJ SALIN ESTOU AQUI DENOVO PARA TE FALAR QUE MEU FETICHE É QUE NEM RAP QUE NEM DRAGÕES GRAVIDAS"
};

LPCSTR Scrolltext = "";
HWND hwndBackground = NULL;
HWND hwndMascot = NULL;

bool buttonpressed(int x, int y, int sizex,int sizey, LPARAM lParam) 
{
    int mouseX = LOWORD(lParam);
    int mouseY = HIWORD(lParam);

    return (mouseX >= x && mouseX <= x + sizex && 
            mouseY >= y && mouseY <= y + sizey);
}

int LoadIMG(HDC hdc, HDC hdcmem, const wchar_t* Filename, int x, int y, int xx, int yy) {
    HBITMAP hBitmap = (HBITMAP)LoadImageW(
        NULL,
        Filename,
        IMAGE_BITMAP, 0, 0,
        LR_LOADFROMFILE
    );

    if (!hBitmap) {
        return 0;  // Failed to load image
    }

    HBITMAP hOldBitmap = (HBITMAP)SelectObject(hdcmem, hBitmap);
    BITMAP bmp;
    GetObject(hBitmap, sizeof(BITMAP), &bmp);
   
    StretchBlt(hdc,
               x, y, xx, yy,  // Destination rectangle
               hdcmem,
               0, 0, bmp.bmWidth, bmp.bmHeight, // Source size
               SRCCOPY);
               
    SelectObject(hdcmem, hOldBitmap);
    DeleteObject(hBitmap);
    return 1; // Success
}

int drawRect(HDC hdc, int x, int y, int xx, int yy, int R, int G, int B) {
    RECT rect = {x, y, xx, yy};  // Create rect with specified coordinates
    HBRUSH hBrush = CreateSolidBrush(RGB(R, G, B));
    if (!hBrush) return 0;  // Failed to create brush
    
    FillRect(hdc, &rect, hBrush);
    DeleteObject(hBrush);
    return 1;  // Success
}

int drawTXT(HDC hdc, HDC Target, int x, int y,int size, LPCSTR Font, LPCSTR Text, int R, int G, int B) {
    HFONT hFont = CreateFont(size, 0, 0, 0, FW_BOLD, FALSE, FALSE, FALSE,
                        ANSI_CHARSET, OUT_TT_PRECIS, CLIP_DEFAULT_PRECIS,
                        DEFAULT_QUALITY, DEFAULT_PITCH | FF_DONTCARE, Font);
    SelectObject(Target, hFont);
    SetTextColor(Target, RGB(R, G, B));
    SetBkMode(Target, TRANSPARENT);
    TextOut(Target, x, y, Text, lstrlen(Text));
    DeleteObject(hFont);
    return 1;
}

static DWORD currentTime = GetTickCount(); // Declare and initialize outside of switch
static float oscillationX = 0.0f;          // Same here
static float oscillationY = 0.0f;          // Same here


LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam) {
    switch (uMsg) {
        case WM_CREATE: {
            
            RECT r;
            GetWindowRect(hwnd, &r);
            int w = r.right - r.left, h = r.bottom - r.top;
            int x = (GetSystemMetrics(SM_CXSCREEN) - w) / 2;
            SetWindowPos(hwnd, HWND_TOP, x, (GetSystemMetrics(SM_CYSCREEN) - h) / 2, w, h, SWP_SHOWWINDOW);
            
            if (modehandler == 0) { SetTimer(hwnd, 1, 2500, NULL); } else { SetTimer(hwnd, 1, 1, NULL); }
            return 0;
        }

        case WM_KEYDOWN: {
            SetWindowPos(hwnd, NULL,  wx, wy + 2, 0, 0,SWP_NOSIZE);

            char ch = MapVirtualKey(wParam, MAPVK_VK_TO_CHAR);



            if(ch && idx < 1023 && typedmax < 15 && wParam != VK_RETURN) {
                buffer[idx++] = ch;
                printf("Key pressed: %c (0x%X)\n", ch, wParam);
                printf("Mult: %s\n", buffer);
                wx += 5;
                typedmax++;
                PlaySound(TEXT("assets\\type.wav"), NULL, SND_FILENAME | SND_ASYNC);
            }
            if(wParam == VK_BACK)
            {
                memset(buffer, 0, sizeof(buffer));  // Reset the buffer
                idx = 0;  // Reset idx
                wx = 300;
                typedmax = 0;
                PlaySound(TEXT("assets\\exex.wav"), NULL, SND_FILENAME | SND_ASYNC);
            } 
            
            if (wParam == VK_RETURN) {
                if (typedmax == 0) {
                    PlaySound(TEXT("assets\\error.wav"), NULL, SND_FILENAME | SND_ASYNC);
                }
            }

            Sleep(10);
            

            return 0;
        }
           
        case WM_TIMER: {
            if (wParam == 1) {
                // Change window style to include all window decorations
                SetWindowLongPtr(hwnd, GWL_STYLE, 
                    WS_OVERLAPPEDWINDOW |    // This includes all window decorations
                    WS_VISIBLE
                );
                
                // Force window to redraw with new style
                SetWindowPos(hwnd, NULL, 
                    wx, wy, 669, 399,
                    SWP_FRAMECHANGED
                );
                modehandler = 1;
                InvalidateRect(hwnd, NULL, TRUE);

                KillTimer(hwnd, 1);

                RECT clientRect;
                GetClientRect(hwnd, &clientRect);  // hwnd is the handle to your window

                windowidth = clientRect.right - clientRect.left;

                Scrolltext = rngtext[rand() % (sizeof(rngtext) / sizeof(rngtext[0]))];

                ShowWindow(hwndTaskbar, SW_HIDE);
                
                SetLayeredWindowAttributes(hwndBackground, 0, 255, LWA_ALPHA);


                SetTimer(hwnd,2,40,NULL);
            }

            if (wParam == 2) {
                if (modehandler == 1) {
                    if (frame > maxframes) {
                        frame = 1;
                    } else { frame++; }

                    if (scrollx < -(scrollsize + 45)) {
                        Scrolltext = rngtext[rand() % (sizeof(rngtext) / sizeof(rngtext[0]))];
                        scrollx = windowidth;
                    }
                    else {
                        scrollx -= 5;
                    }

                    swprintf(filefromframe, L"assets\\LGPL\\frame%d.bmp", frame);
                    InvalidateRect(hwnd, NULL, FALSE);
                }
                if (modehandler == 2) {
                    if (frame > 1) {
                        frame = 1;
                    } else { frame++; }

                    swprintf(filefromframe, L"assets\\BGDE\\f%d.bmp", frame);
                    InvalidateRect(hwnd, NULL, FALSE);
                }




                currentTime = GetTickCount();

                // first is amplitude,second is oscilation
                oscillationX = sin(currentTime * 0.001) * 0.05;  // Horizontal movement based on time
                oscillationY = cos(currentTime * 0.001) * 0.05; // Vertical movement based on time

                        // Apply the new position to the window
                SetWindowPos(hwnd, HWND_TOP, 
                            wx + (int)(oscillationX * 70),   // Scale the oscillation
                            wy + (int)(oscillationY * 50),   // Scale the oscillation
                            0, 0, SWP_NOREDRAW | SWP_NOSIZE);


            }
            return 0;
        }

        case WM_DESTROY:
            ShowWindow(hwndTaskbar, SW_SHOW);
            PostQuitMessage(0);
            return 0;


        case WM_LBUTTONDOWN:
        {
			if (buttonpressed(259, 184, 150, 30, lParam) && modehandler == 2) 
            {
                Sleep(1000);
                modehandler = 1;
            }
        }
        break;

        case WM_PAINT: {
            PAINTSTRUCT ps;
            HDC hdc = BeginPaint(hwnd, &ps);
            HDC hdcmem = CreateCompatibleDC(hdc);

            HDC hBUF = CreateCompatibleDC(hdc);
            HBITMAP hbmBuffer = CreateCompatibleBitmap(hdc, 699, 399);
            SelectObject(hBUF, hbmBuffer);

            if (modehandler == 0) 
            {

                LoadIMG(hdc,hdcmem,L"assets\\SPLASH.bmp",0,0,500,700);
            }


            if(modehandler == 1) {


                LoadIMG(hBUF,hdcmem,filefromframe,0,0,699,399);







                drawRect(hBUF,0,32,699,75,0,0,0);
                drawRect(hBUF,596,0,636,399,0,0,0); // i hate relative position,its like uhh picking rectangle points

                drawTXT(hdc,hBUF,10,37,34,"simsun","SUPERMENUX",0,255,0);

                drawTXT(hdc,hBUF,21,88,20,"Arial","Conectar ao servidor",255,255,255);

                drawTXT(hdc,hBUF,25,119,35,"Arial","IP",255,255,255);

                drawTXT(hdc,hBUF,27,158,20,"Arial",buffer,255,255,255);

                drawRect(hBUF,21,180,189,178,255,255,255);

                
                HFONT hFont = CreateFont(30, 0, 0, 0, FW_BOLD, FALSE, FALSE, FALSE,
                                        ANSI_CHARSET, OUT_TT_PRECIS, CLIP_DEFAULT_PRECIS,
                                        DEFAULT_QUALITY, DEFAULT_PITCH | FF_DONTCARE, TEXT("Arial"));
                SelectObject(hBUF, hFont);  // Select the font into hBUF

                SetTextColor(hBUF, RGB(255, 255, 255)); // Set the text color
                SetBkMode(hBUF, TRANSPARENT); // Make background transparent
                SIZE textSize;
                GetTextExtentPoint32(hBUF, Scrolltext, lstrlen(Scrolltext), &textSize);
                scrollsize = textSize.cx;
                TextOut(hBUF, scrollx, (330 + sin(currentTime * 0.003) * 2), Scrolltext, lstrlen(Scrolltext));


                


                BitBlt(hdc, 0, 0, 669, 399, hBUF, 0, 0, SRCCOPY);

            }
            
            if(modehandler == 2) {


                LoadIMG(hBUF,hdcmem,filefromframe,0,0,699,399);

                LoadIMG(hBUF,hdcmem,L"assets\\BGDE\\HELP.bmp",(rand() % 3) + 259,(rand() % 3) + 184,150,30);
                


                BitBlt(hdc, 0, 0, 669, 399, hBUF, 0, 0, SRCCOPY);

            }



            

            EndPaint(hwnd, &ps);
            DeleteDC(hdcmem);
            DeleteObject(hBUF);

            return 0;
        }
    }
    return DefWindowProc(hwnd, uMsg, wParam, lParam);
}

LRESULT CALLBACK BackgroundWindowProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam) {
    switch (msg) {
        case WM_CREATE: {  // Added curly braces
            LONG lStyle = GetWindowLong(hwnd, GWL_STYLE);
            lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU);
            SetWindowLong(hwnd, GWL_STYLE, lStyle);
            SetWindowPos(hwnd, NULL, 0,0,0,0, SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOOWNERZORDER);
            break;
        }

        case WM_DESTROY:
            PostQuitMessage(0);
            break;
        case WM_PAINT: {
            PAINTSTRUCT ps2;
            HDC hdc2 = BeginPaint(hwnd, &ps2);
            HDC hdcmem2 = CreateCompatibleDC(hdc2);
            LoadIMG(hdc2, hdcmem2, L"assets\\BG.bmp", 0, 0, GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN));
            EndPaint(hwnd, &ps2);
            break;
        }
        default:
            return DefWindowProc(hwnd, msg, wParam, lParam);
    }
    return 0;
}

int xxiri = GetSystemMetrics(SM_CXSCREEN);
const wchar_t* spritenumber = L"assets\\POSES\\walk.bmp";

LRESULT CALLBACK Mascothandler(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam) {
    switch (msg) {
        case WM_CREATE: {  // Added curly braces
            LONG lStyle = GetWindowLong(hwnd, GWL_STYLE);
            lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU);
            SetWindowLong(hwnd, GWL_STYLE, lStyle);
            SetWindowPos(hwnd, NULL, 0,0,0,0, SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOOWNERZORDER);
            SetTimer(hwnd,1,100,NULL);
            break;
        }
        case WM_DESTROY:

            PostQuitMessage(0);
            break;
        case WM_TIMER:
            if(wParam == 1) {
                if (modehandler != 1) { break; }
                if (xxiri < GetSystemMetrics(SM_CXSCREEN) - 145) {
                    KillTimer(hwnd,1);
                    spritenumber = L"assets\\POSES\\vent.bmp";
                    InvalidateRect(hwnd, NULL, TRUE);
                    SetTimer(hwnd,2,1000,NULL);
                }
                else {
                    SetWindowPos(hwnd, HWND_TOP, 
                                xxiri,   // Scale the oscillation
                                GetSystemMetrics(SM_CYSCREEN) - 223,   // Scale the oscillation
                                0, 0, SWP_NOREDRAW | SWP_NOSIZE);
                    xxiri -= 5;
                }
            }
            if(wParam == 2) {
                KillTimer(hwnd,2);
                spritenumber = L"assets\\POSES\\look.bmp";
                InvalidateRect(hwnd, NULL, TRUE);
            }
            break;
        case WM_PAINT: {
            PAINTSTRUCT ps2;
            HDC hdc2 = BeginPaint(hwnd, &ps2);
            HDC hdcmem2 = CreateCompatibleDC(hdc2);
            LoadIMG(hdc2, hdcmem2, spritenumber, 0, 0, 139, 223);
            EndPaint(hwnd, &ps2);
            break;
        }
        default:
            return DefWindowProc(hwnd, msg, wParam, lParam);
    }
    return 0;
}


int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow) {


    const char CLASS_NAME[] = "SUPERMENUXFOR";
    WNDCLASS wc = { };
    wc.lpfnWndProc = WindowProc;
    wc.hInstance = hInstance;
    wc.lpszClassName = CLASS_NAME;
    wc.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
    RegisterClass(&wc);

    WNDCLASS wcBackground = {};
    wcBackground.lpfnWndProc = BackgroundWindowProc;
    wcBackground.hInstance = hInstance;
    wcBackground.lpszClassName = "BGCLASS";
    RegisterClass(&wcBackground);


    WNDCLASS wcMASCOT = {};
    wcMASCOT.lpfnWndProc = Mascothandler;
    wcMASCOT.hInstance = hInstance;
    wcMASCOT.lpszClassName = "MASCT";
    RegisterClass(&wcMASCOT);

    hwndBackground = CreateWindowEx(
        WS_EX_LAYERED, // Extra styles (Layered for transparency)
        wcBackground.lpszClassName,
        "BG",
        WS_OVERLAPPEDWINDOW | WS_VISIBLE, 
        0, 0, GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN), // Full screen
        NULL, NULL, hInstance, NULL);


    hwndMascot = CreateWindowEx(
        WS_EX_LAYERED, // Extra styles (Layered for transparency)
        wcMASCOT.lpszClassName,
        "MASCOTCLASS",
        WS_OVERLAPPEDWINDOW | WS_VISIBLE, 
        GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN) - 223, 139, 223, // Full screen
        NULL, NULL, hInstance, NULL);

    HWND hwnd = CreateWindowEx(
        WS_EX_LAYERED,
        CLASS_NAME,
        "SUPERMENUX || GDI+ MODE",
        WS_POPUP,
        wx, wy, 500, 700,
        NULL, NULL, hInstance, NULL
    );

    if (hwnd == NULL) {
        return 0;
    }

    SetLayeredWindowAttributes(hwnd, 0, 255, LWA_ALPHA);
    SetLayeredWindowAttributes(hwndBackground, 0, 0, LWA_ALPHA);
    SetLayeredWindowAttributes(hwndMascot, RGB(0, 0, 255), 255, LWA_COLORKEY); // ITS BONZI TIME
    ShowWindow(hwndBackground, nCmdShow);
    UpdateWindow(hwndBackground);

    ShowWindow(hwnd, nCmdShow);
    UpdateWindow(hwnd);

    ShowWindow(hwndMascot, nCmdShow);
    UpdateWindow(hwndMascot);


    MSG msg = { };
    while (GetMessage(&msg, NULL, 0, 0)) {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return 0;
}
// SharpShellNativeBridge.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "resource.h"
#include "Shobjidl.h"



extern "C"  int __declspec(dllexport) CallAddPropSheetPage(LPFNSVADDPROPSHEETPAGE lpfnAddPage, HPROPSHEETPAGE hProp, LPARAM lParam)
							   {
								   int res = (*lpfnAddPage)(hProp, lParam);
								   if(!res)
								   {
									   DestroyPropertySheetPage(hProp);
								   }
								   return res;
							   }
extern "C"  LPCTSTR __declspec(dllexport) GetProxyHostTemplate()
							   {
								   return MAKEINTRESOURCE(IDD_PROXY_HOST);
							   }

extern "C"  void __declspec(dllexport) CreatePropertySheet(PROPSHEETHEADER* pPSH)
							   {
								   PropertySheet(pPSH);
							   }
﻿using System;

namespace WinUIToy3.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);
}
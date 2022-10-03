﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIStudio.Common.Cache;

/// <summary>
/// 缓存接口
/// </summary>
public interface ICache
{
    /// <summary>
    /// 用于在 key 存在时删除 key
    /// </summary>
    /// <param name="key">键</param>
    long Del(params string[] key);

    /// <summary>
    /// 用于在 key 存在时删除 key
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    Task<long> DelAsync(params string[] key);

    /// <summary>
    /// 用于在 key 模板存在时删除
    /// </summary>
    /// <param name="pattern">key模板</param>
    /// <returns></returns>
    Task<long> DelByPatternAsync(string pattern);

    /// <summary>
    /// 检查给定 key 是否存在
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    bool Exists(string key);

    /// <summary>
    /// 检查给定 key 是否存在
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    Task<bool> ExistsAsync(string key);

    /// <summary>
    /// 获取指定 key 的值
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    string Get(string key);

    /// <summary>
    /// 获取指定 key 的值
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">键</param>
    /// <returns></returns>
    T Get<T>(string key);

    /// <summary>
    /// 获取指定 key 的值
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    Task<string> GetAsync(string key);

    /// <summary>
    /// 获取指定 key 的值
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">键</param>
    /// <returns></returns>
    Task<T> GetAsync<T>(string key);

    /// <summary>
    /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    bool Set(string key, object value);

    /// <summary>
    /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="expire">有效期</param>
    bool Set(string key, object value, TimeSpan expire);

    /// <summary>
    /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    Task<bool> SetAsync(string key, object value);

    /// <summary>
    /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="expire">有效期</param>
    /// <returns></returns>
    Task<bool> SetAsync(string key, object value, TimeSpan expire);

    /// <summary>
    /// 获取所有缓存
    /// </summary>
    /// <returns></returns>
    List<string> GetAllKeys();
}

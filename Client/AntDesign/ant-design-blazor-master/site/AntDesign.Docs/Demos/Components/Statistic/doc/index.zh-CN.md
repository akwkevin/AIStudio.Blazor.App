---
category: Components
subtitle: 统计数值
type: 数据展示
title: Statistic
cover: https://gw.alipayobjects.com/zos/antfincdn/rcBNhLBrKbE/Statistic.svg
---

展示统计数值。

## 何时使用

- 当需要突出某个或某组数字时。
- 当需要展示带描述的统计类数据时使用。

## API

#### Statistic

| 参数             | 说明             | 类型                         | 默认值 | 版本 |
| ---------------- | ---------------- | ---------------------------- | ------ | ---- |
| decimalSeparator | 设置小数点       | string                       | .      |      |
| groupSeparator   | 设置千分位标识符 | string                       | ,      |      |
| precision        | 数值精度         | int                          | -      |      |
| prefix           | 设置数值的前缀   | string\|RenderFragment       | -      |      |
| suffix           | 设置数值的后缀   | string\|RenderFragment       | -      |      |
| title            | 数值的标题       | string\|RenderFragment       | -      |      |
| value            | 数值内容         | decimal\|Datetime\|TimeSpan  | -      |      |
| valueStyle       | 设置数值的样式   | string                       | -      |      |

#### Statistic.Countdown

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| format | 格式化倒计时展示，参考 [TimeSpan](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/custom-timespan-format-strings?WT.mc_id=DT-MVP-5003987) | string | @"hh\:mm\:ss" |  |
| onFinish | 倒计时完成时触发 | () => void | - |  |
| prefix | 设置数值的前缀 | string \| RenderFragment | - |  |
| suffix | 设置数值的后缀 | string \| RenderFragment | - |  |
| title | 数值的标题 | string \| RenderFragment | - |  |
| value | 数值内容 | Datetime \| TimeSpan | - |  |
| valueStyle | 设置数值的样式 | string | - |  |

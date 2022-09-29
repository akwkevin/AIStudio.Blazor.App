---
order: 9
debug: true
title:
  zh-CN: 自定义状态
  en-US: Status
---

## zh-CN

使用 `status` 为 TreeSelect 添加状态，可选 `error` 或者 `warning`。

## en-US

Add status to TreeSelect with `status`, which could be `error` or `warning`.

```tsx
import React from 'react';
import { TreeSelect, Space } from 'antd';

const App: React.FC = () => (
  <Space direction="vertical" style={{ width: '100%' }}>
    <TreeSelect status="error" style={{ width: '100%' }} placeholder="Error" />
    <TreeSelect
      status="warning"
      style={{ width: '100%' }}
      multiple
      placeholder="Warning multiple"
    />
  </Space>
);

export default App;
```

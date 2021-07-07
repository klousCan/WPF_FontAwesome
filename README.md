# WPF 像CSS一样使用 Font Awesome 图标字体

------

 - 编写目的
   WPF中使用这种图标字体不免会出现可读性差的问题，现阶段网络上有的大部分实现方式都是建立枚举，我感觉这样后续维护起来有些麻烦，需要重新手动将图标名称和unicode编码进行对应。
例如：
```
    <TextBlock Text="&#xf01a;" Style="{DynamicResource FontAwesome}" />
```
这种代码在不运行时不会知道这是个什么图标，想在图标库官方上查找一下都困难。

------

- 实现思路 
  1.加载网页使用的CSS文件,将CSS中的名称与unicode的对应关系加载到本地
  2.实现 IValueConverter 进行图标名称和unicode的转换

实现后代码样例:
```
<TextBlock Text="{Binding Converter={StaticResource FontAwesomeConvert},ConverterParameter='fa-close'}" Style="{DynamicResource FontAwesome}" ></TextBlock>
```
------

- 源码下载：
https://github.com/rongcan/WPF_FontAwesome.git   
记得点赞和推荐，你的赞和推荐就是我最好的动力

------
个人能力有限，本文内容仅供学习、探讨，欢迎指正、交流。

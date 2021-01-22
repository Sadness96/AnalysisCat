## 解析 Asterix 数据
### 简介
Asterix 是当前用于描述信息结构的欧控标准（[EUROCONTROL STANDARD](https://www.eurocontrol.int/publication/eurocontrol-standard-inputs-economic-analyses)），它是All Purpose Structured Eurocontrol Radar Information Exchange的首字母組合而成。Asterix是由RSSP在1986年7月举行的第15次会议提出的。现在Asterix由STFRDE（Surveillance Task Force for Radar Data Exchange）负责。Asterix数据格式已成为世界上雷达监视相关数据传输的主要格式。

### 项目简介
工作中需要对接机场航空器运动数据，学习使用这种数据的解析，这个程序使用的所有测试数据均以过期失效，不包含任何公司业务，所有文档与解析参考均来自于网络公开内容。
解析的数据并不完全，仅解析重要部分数据，提供解析方式的参考。
当前解析的数据类型有：
* [Cat020](https://www.eurocontrol.int/sites/default/files/2019-06/cat020-asterix-mlt-messages-part-14.pdf) 多点定位(MLAT)，解析细节参考[博客文章](https://liujiahua.com/blog/2019/09/09/csharp-Cat020015/)
* [Cat021](https://www.eurocontrol.int/sites/default/files/content/documents/nm/asterix/20150615-asterix-adsbtr-cat021-part12-v2.4.pdf) ADS-B，解析细节参考[博客文章](https://liujiahua.com/blog/2019/08/19/csharp-Cat021026/)
* [Cat062](https://www.eurocontrol.int/sites/default/files/content/documents/nm/asterix/cat062p9ed118.pdf) 二次雷达，解析细节参考[博客文章](https://liujiahua.com/blog/2019/10/15/csharp-Cat062118/)

<img src="/Images/MainWindow.jpg"/>

### 测试数据示例
#### Cat020
##### 报文密文
``` txt
14 00 46 FF 0F 01 84 16 07 41 10 A1 A0 BB 00 57 8B 48 01 44 DC F6 00 17 06 00 1F AD 0E F2 02 78 10 45 80 0C 54 F2 DB 3C 60 00 02 20 40 19 98 D0 00 00 00 00 00 01 00 0C 00 0C 00 03 00 06 00 05 00 05 A1 A0 C2 00
```

##### 报文解析
``` json
{
    "I020/140":"2021/1/22 6:59:13.460",
    "I020/041":[
        30.77721118927002,
        114.20969367027283
    ],
    "I020/042":[
        2947,
        4054.5
    ],
    "I020/161":3826,
    "I020/245":"CES2631",
    "I020/110":12.5
}
```

#### Cat020
##### 报文密文
``` txt
15002efba1df80000100302327660055a0b60144ae0a7802610006080388000a077e043e0d33b3c72de000800002
```

##### 报文解析
``` json
{
    "I021/030":"2021/1/22 4:59:58.796",
    "I021/130":[
        114.145255647124,
        30.103515219435998
    ],
    "I021/080":"780261",
    "I021/157":62.5,
    "I021/170":"CSN3127 "
}
```

#### Cat062
##### 报文密文
``` txt
3E0034BB7D25040203000E584F003806E501460641FD2601B70D4A000D33B3C37E2080780CCB000601000550000028002A003E04
```

##### 报文解析
``` json
{
    "I062/070":"2021/1/22 2:2:24.617",
    "I062/105":[
        19.696968197822571,
        114.61796343326569
    ],
    "I062/245":"CSN3078"
}
```
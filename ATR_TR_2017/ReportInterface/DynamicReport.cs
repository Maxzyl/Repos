
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Serialization;
using Microsoft.Reporting;
using ReportInterface;
using Microsoft.Reporting.WinForms;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace ReportInterface
{
    public class ReportColoumStyle : MarshalByRefObject
    {
        public string ColoumName { get; set; }
        public float ColoumWidth { get; set; }
        public TextAlign TextAlign { get; set; }
       
    }

    public enum TextAlign
    {
        Left, Center, Right
    }
    public class ReportItem : MarshalByRefObject
    {
        public string DataSetName { get; set; }
        public string DataSetString { get; set; }
        public string TablixString { get; set; }

        public string DataSetString2 { get; set; }
        public string TablixString2 { get; set; }

        public string PageFootString { get; set; }
        public string RectangleString { get; set; }
        public DataTable Data { get; set; }
        public string PictureString { get; set; }

        public string DataSet
        {
            get
            {
                return "    <DataSet Name=\"@DataSetNameData\">" +
                      "       <Fields>@Fields</Fields>" +
                      "       <Query>" +
                      "           <DataSourceName>DummyDataSource</DataSourceName>" +
                      "           <CommandText />" +
                      "       </Query>" +
                      "    </DataSet>";
            }
        }

        public string Tablix
        {
            get
            {
                return " <Tablix Name=\"Tablix@DataSetName\">" +
                        " <TablixBody>" +
                        "   <TablixColumns>@TablixColumns</TablixColumns>" +
                        "   <TablixRows>" +

                        #region --表头
                         "<TablixRow>" +
                         " <Height>1cm</Height>" +
                         " <TablixCells>" +
                         "   <TablixCell>" +
                          "     <CellContents>" +
                          "       <Textbox Name=\"Textbox@DataSetName\">" +
                           "        <CanGrow>true</CanGrow>" +
                            "       <KeepTogether>true</KeepTogether>" +
                             "      <Paragraphs>" +
                             "        <Paragraph>" +
                              "         <TextRuns>" +
                              "           <TextRun>" +
                               "            <Value>@title</Value>" +
                                 "          <Style>" +
                               "              <FontFamily>黑体</FontFamily>" +
                               "              <FontSize>11pt</FontSize>" +
                                "             <FontWeight>Bold</FontWeight>" +
                                "             <Color>Black</Color>" +
                               "            </Style>" +
                               "          </TextRun>" +
                                "       </TextRuns>" +
                               "        <Style>" +
                                "         <TextAlign>Left</TextAlign>" +
                                 "      </Style>" +
                                 "    </Paragraph>" +
                                "   </Paragraphs>" +
                                "   <rd:DefaultName>Textbox1</rd:DefaultName>" +
                               "    <Style>" +
                               "      <Border><Style>None</Style></Border>" +
                               "      <VerticalAlign>Bottom</VerticalAlign>"+
                               //"      <VerticalAlign>Middle</VerticalAlign>" +
                                "     <PaddingLeft>0pt</PaddingLeft>" +
                                "     <PaddingRight>0pt</PaddingRight>" +
                                 "    <PaddingTop>0pt</PaddingTop>" +
                                "     <PaddingBottom>0pt</PaddingBottom>" +
                               "    </Style>" +
                              "   </Textbox>" +
                             "    <ColSpan>@count</ColSpan>" +
                            "   </CellContents>" +
                           "  </TablixCell>" +
                           "@Cell" +
                        "   </TablixCells>" +
                       "  </TablixRow>" +

                #endregion


                        "      <TablixRow>" +
                        "        <Height>1cm</Height>" +
                        "        <TablixCells>@TablixHeader" +
                        "</TablixCells> " +
                        "      </TablixRow>" +
                        "      <TablixRow>" +
                        "        <Height>1cm</Height>" +
                        "        <TablixCells>@TablixCells" +
                        "</TablixCells> " +
                        "      </TablixRow>" +
                        "   </TablixRows>" +
                        " </TablixBody>" +
                        " <TablixColumnHierarchy>" +
                        "   <TablixMembers>@TablixMembers</TablixMembers>" +
                        " </TablixColumnHierarchy>" +
                        " <TablixRowHierarchy> " +
                        "   <TablixMembers> " +
                        "      <TablixMember>" +
                        "         <KeepWithGroup>After</KeepWithGroup>" +

                        "         <RepeatOnNewPage>true</RepeatOnNewPage>"+ 

                        "      </TablixMember>" +
                        "      <TablixMember>" +
                        "         <KeepWithGroup>After</KeepWithGroup>" +

                        "         <RepeatOnNewPage>true</RepeatOnNewPage>" + 

                        "      </TablixMember>" +
                        "      <TablixMember>" +
                        "         <Group Name=\"Details@DataSetName\" />" +
                        "      </TablixMember>" +
                        "  </TablixMembers> " +
                        " </TablixRowHierarchy>" +
                        " <DataSetName>@DataSetNameData</DataSetName>" +
                        "   <Top>@TopPositioncm</Top>" +
                        "   <Left>0.5cm</Left> " +
                        "   <Height>1.70812cm</Height>" +
                        "   <Width>21cm</Width>" +
                        " <Style> " +
                        " <BottomBorder>" +
                        "    <Color>Black</Color>" +
                        "    <Style>Solid</Style>" +
                        "    <Width>0.6pt</Width>" +
                        "  </BottomBorder>" +
                        " </Style>" +
                        " </Tablix>";


            }
        }

        public string picture
        {
            get
            {//15.58396,9
                return " <Tablix Name=\"Tablix@DataSetName\">" +
                          " <TablixBody>" +
                          "   <TablixColumns> <TablixColumn>" +
                          "  <Width>15.58396cm</Width>" +
                          " </TablixColumn></TablixColumns>" +
                          "   <TablixRows>" +
                           "<TablixRow>" +
                           " <Height>1cm</Height>" +
                           " <TablixCells>" +
                           "   <TablixCell>" +
                            "     <CellContents>" +
                            "       <Textbox Name=\"Textbox@DataSetName\">" +
                             "        <CanGrow>true</CanGrow>" +
                              "       <KeepTogether>true</KeepTogether>" +
                               "      <Paragraphs>" +
                               "        <Paragraph>" +
                                "         <TextRuns>" +
                                "           <TextRun>" +
                                 "            <Value>@title</Value>" +
                                   "          <Style>" +
                                 "              <FontFamily>Tahoma</FontFamily>" +
                                 "              <FontSize>11pt</FontSize>" +
                                  "             <FontWeight>Bold</FontWeight>" +
                                 "            </Style>" +
                                 "          </TextRun>" +
                                  "       </TextRuns>" +
                                 "        <Style>" +
                                  "         <TextAlign>Left</TextAlign>" +
                                   "      </Style>" +
                                   "    </Paragraph>" +
                                  "   </Paragraphs>" +
                                  "   <rd:DefaultName>Textbox1</rd:DefaultName>" +
                                 "    <Style>" +
                                  "     <VerticalAlign>Bottom</VerticalAlign>" +
                                  "     <PaddingLeft>2pt</PaddingLeft>" +
                                  "     <PaddingRight>2pt</PaddingRight>" +
                                   "    <PaddingTop>2pt</PaddingTop>" +
                                  "     <PaddingBottom>2pt</PaddingBottom>" +
                                 "    </Style>" +
                                "   </Textbox>" +
                              "   </CellContents>" +
                             "  </TablixCell>" +
                          "   </TablixCells>" +
                         "  </TablixRow>" +

                          "      <TablixRow>" +
                          "        <Height>9cm</Height>" +
                          "        <TablixCells> <TablixCell>" +
                                  "<CellContents>" +
                                   " <Image Name=\"Image@DataSetName\">" +
                                 "  <Source>External</Source>" +
                                   "   <Value>@name</Value>" +
                                   "   <Sizing>FitProportional</Sizing>" +
                                   "  <Height>9.39271cm</Height>" +
                                   "  <Width>15.58396cm</Width>" +
                                   "   <Style>" +
                                   "     <Border>" +
                                   "       <Style>None</Style>" +
                                   "    </Border>" +
                                   "   </Style>" +
                                   " </Image>" +
                                  "</CellContents>" +
                                "</TablixCell>" +
                          "</TablixCells> " +
                          "      </TablixRow>" +
                          "   </TablixRows>" +
                          " </TablixBody>" +
                          " <TablixColumnHierarchy>" +
                          "   <TablixMembers>  <TablixMember /></TablixMembers>" +
                          " </TablixColumnHierarchy>" +
                          " <TablixRowHierarchy> " +
                          "   <TablixMembers> " +
                          "      <TablixMember>" +
                          "         <KeepWithGroup>After</KeepWithGroup>" +
                          "      </TablixMember>" +
                          "      <TablixMember>" +
                          "         <Group Name=\"Details@DataSetName\" />" +
                          "      </TablixMember>" +
                          "  </TablixMembers> " +
                          " </TablixRowHierarchy>" +
                          " <DataSetName>@DataSetNameData</DataSetName>" +
                          "   <Top>@TopPositioncm</Top>" +
                          "   <Left>0.5cm</Left> " +
                          "   <Height>1.70812cm</Height>" +
                          "   <Width>21cm</Width>" +
                          " <Style> " +
                          "   <Border> " +
                          "     <Style>None</Style> " +
                          "   </Border>" +
                          " </Style>" +
                          " </Tablix>";
            }
        }

        public string Tablix2
        {
            get
            {
                return  " <Tablix Name=\"Tablix@DataSetName\">" +
                        " <TablixBody>" +
                        "   <TablixColumns>@TablixColumns</TablixColumns>" +
                        "   <TablixRows>" +

                        "      <TablixRow>" +
                        "        <Height>1cm</Height>" +
                        "        <TablixCells>@TablixHeader" +
                        "</TablixCells> " +
                        "      </TablixRow>" +
                        "      <TablixRow>" +
                        "        <Height>1cm</Height>" +
                        "        <TablixCells>@TablixCells" +
                        "</TablixCells> " +
                        "      </TablixRow>" +
                        "   </TablixRows>" +
                        " </TablixBody>" +
                        " <TablixColumnHierarchy>" +
                        "   <TablixMembers>@TablixMembers</TablixMembers>" +
                        " </TablixColumnHierarchy>" +
                        " <TablixRowHierarchy> " +
                        "   <TablixMembers> " +
                        "      <TablixMember>" +
                        "         <KeepWithGroup>After</KeepWithGroup>" +
                        "         <RepeatOnNewPage>true</RepeatOnNewPage>" +
                        "      </TablixMember>" +
                        "      <TablixMember>" +
                        "         <Group Name=\"Details@DataSetName\" />" +
                        "      </TablixMember>" +
                        "  </TablixMembers> " +
                        " </TablixRowHierarchy>" +
                        " <DataSetName>@DataSetNameData</DataSetName>" +
                        "   <Top>8cm</Top>" +
                        "   <Left>0.5cm</Left> " +
                        "   <Height>1.70812cm</Height>" +
                        "   <Width>21cm</Width>" +
                        " </Tablix>";
            }
        }
    }

    public class DynamicReport : MarshalByRefObject
    {
        #region --页脚
        protected static string PageFoot =

                 "  <Textbox Name=\"Textbox2222\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>确认:__________________________</Value><Style /></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                 "    <rd:DefaultName>Textbox1</rd:DefaultName><Top>1cm</Top><Left>0.5cm</Left><Height>1cm</Height><Width>6cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                 "  </Textbox>" +

                 "  <Textbox Name=\"Textbox3333\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>审核:__________________________</Value><Style /></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                 "    <rd:DefaultName>Textbox1</rd:DefaultName><Top>1cm</Top><Left>10.5cm</Left><Height>1cm</Height><Width>6cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                 "  </Textbox>" +

                 "  <Textbox Name=\"Textbox@DataSetName\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>=\"第\" &amp; Globals!PageNumber &amp; \"页 共\" &amp; Globals!TotalPages &amp; \"页\"</Value><Style /></TextRun></TextRuns><Style><TextAlign>Left</TextAlign></Style></Paragraph></Paragraphs>" +
                 "   <rd:DefaultName>Textbox@DataSetName</rd:DefaultName><Top>2.2cm</Top><Left>0.5cm</Left><Height>0.8cm</Height><Width>2.5cm</Width><Style><Border><Style>None</Style></Border><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                 "  </Textbox>" +

                 "  <Image Name=\"Image@pagefoot\"><Source>External</Source><Sizing>Fit</Sizing><Value>@str</Value><Visibility><Hidden>@show</Hidden></Visibility><Top>2cm</Top><Left>11cm</Left><Height>1cm</Height><Width>5cm</Width><Style><Border><Style>None</Style></Border></Style>" +
                 "  </Image>";

        #endregion

        #region --封面

        protected static string Rectangle = "<Rectangle Name=\"Rectangle1\"><ReportItems>" +

                    "<Line Name=\"Line1\"> <Top>1cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border> <Style>Solid</Style></Border></Style></Line>" +
                 
                    "<Line Name=\"Line2\"> <Top>7cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border><Style>Solid</Style></Border></Style></Line>" +
                  
                    //"<Line Name=\"Line3\"> <Top>8cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border>   <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                    //"<Line Name=\"Line4\"> <Top>9cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border>   <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                    //"<Line Name=\"Line5\"> <Top>10cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border>  <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                    //"<Line Name=\"Line6\"> <Top>11cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border>  <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                    //"<Line Name=\"Line7\"> <Top>12cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border>  <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                    //"<Line Name=\"Line8\"> <Top>13cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border>  <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                    //"<Line Name=\"Line9\"> <Top>14cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border>  <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                    //"<Line Name=\"Line10\"> <Top>15cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border> <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                    //"<Line Name=\"Line11\"> <Top>16cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border> <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                    //"<Line Name=\"Line12\"> <Top>17cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border> <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                    //"<Line Name=\"Line13\"> <Top>18cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border> <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                    //"<Line Name=\"Line14\"> <Top>19cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border> <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                    //"<Line Name=\"Line15\"> <Top>21cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border> <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                   
                    //"<Line Name=\"Line16\"> <Top>8cm</Top><Left>7.6cm</Left><Height>13cm</Height><Width>0cm</Width><Style><Border>  <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                    //"<Line Name=\"Line17\"> <Top>8cm</Top><Left>0.5cm</Left><Height>13cm</Height><Width>0cm</Width><Style><Border>  <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                    //"<Line Name=\"Line18\"> <Top>8cm</Top><Left>16.47cm</Left><Height>13cm</Height><Width>0cm</Width><Style><Border>  <Color>LightGrey</Color><Style>Solid</Style></Border></Style></Line>" +
                  
                    //"<Line Name=\"Line19\"> <Top>22.1cm</Top><Left>0.5cm</Left><Height>0cm</Height><Width>16cm</Width><ZIndex>10</ZIndex><Style><Border> <Style>Solid</Style></Border></Style></Line>" +

                   "<Image Name=\"Image@Rectangle\"><Source>External</Source><Value>@str</Value> <Visibility><Hidden>@show</Hidden></Visibility><Sizing>Fit</Sizing><Top>0cm</Top><Left>4cm</Left><Height>0.8cm</Height>" +
                    "  <Width>10cm</Width><Style><Border><Style>None</Style></Border></Style></Image>" +

                    "  <Textbox Name=\"Textbox66\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>@TitleName</Value><Style><FontSize>36pt</FontSize><FontWeight>Bold</FontWeight></Style></TextRun></TextRuns><Style><TextAlign>Center</TextAlign></Style></Paragraph></Paragraphs>" +
                    "    <rd:DefaultName>Textbox1</rd:DefaultName><Top>2cm</Top><Left>0cm</Left><Height>1cm</Height><Width>16cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    "  </Textbox>" +

                    "  <Textbox Name=\"Textbox67\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>测试报告</Value><Style><FontSize>36pt</FontSize><FontWeight>Bold</FontWeight></Style></TextRun></TextRuns><Style><TextAlign>Center</TextAlign></Style></Paragraph></Paragraphs>" +
                    "    <rd:DefaultName>Textbox1</rd:DefaultName><Top>3.5cm</Top><Left>0cm</Left><Height>1.5cm</Height><Width>16cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    "  </Textbox>" +

                    "  <Textbox Name=\"Textbox68\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>测试结果</Value><Style><FontSize>22pt</FontSize><FontWeight>Bold</FontWeight></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    "    <rd:DefaultName>Textbox1</rd:DefaultName><Top>6cm</Top><Left>2cm</Left><Height>1cm</Height><Width>4cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    "  </Textbox>" +

                    "  <Textbox Name=\"Textbox69\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>合格</Value><Style><FontSize>22pt</FontSize><FontWeight>Bold</FontWeight></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    "    <rd:DefaultName>Textbox1</rd:DefaultName><Top>6cm</Top><Left>11cm</Left><Height>1cm</Height><Width>2cm</Width><Visibility><Hidden>=iif(\"@Result\"=\"不合格\",true,iif(\"@Result\"=\"fail\",true,false))</Hidden></Visibility><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    "  </Textbox>" +

                    "  <Textbox Name=\"Textbox689\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>/</Value><Style><FontSize>22pt</FontSize><FontWeight>Bold</FontWeight></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    "    <rd:DefaultName>Textbox1</rd:DefaultName><Top>6cm</Top><Left>13cm</Left><Height>1cm</Height><Width>0.5cm</Width><Visibility><Hidden>=iif(\"@Result\"=\"不合格\",true,iif(\"@Result\"=\"fail\",true,iif(\"@Result\"=\"合格\",true,iif(\"@Result\"=\"pass\",true,false))))</Hidden></Visibility><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    "  </Textbox>" +

                    "  <Textbox Name=\"Textbox669\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>不合格</Value><Style><FontSize>22pt</FontSize><FontWeight>Bold</FontWeight><Color>=iif(\"@Result\"=\"不合格\",\"Red\",iif(\"@Result\"=\"fail\",\"Red\",\"Black\"))</Color> </Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    "    <rd:DefaultName>Textbox1</rd:DefaultName><Top>6cm</Top><Left>13cm</Left><Height>1cm</Height><Width>3cm</Width> <Visibility><Hidden>=iif(\"@Result\"=\"合格\",true,iif(\"@Result\"=\"pass\",true,false))</Hidden></Visibility><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    "  </Textbox>" +






                    //"  <Textbox Name=\"Textbox65\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>内容</Value><Style><FontSize>11pt</FontSize></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>8cm</Top><Left>0.5cm</Left><Height>1cm</Height><Width>7cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox64\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>描述</Value><Style><FontSize>11pt</FontSize></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>8cm</Top><Left>8cm</Left><Height>1cm</Height><Width>6cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox63\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>待测物批号</Value><Style><FontSize>11pt</FontSize></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>9cm</Top><Left>0.5cm</Left><Height>1cm</Height><Width>7cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox62\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>待测物捆号</Value><Style><FontSize>11pt</FontSize></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>10cm</Top><Left>0.5cm</Left><Height>1cm</Height><Width>7cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox61\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>待测物长度</Value><Style><FontSize>11pt</FontSize></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>11cm</Top><Left>0.5cm</Left><Height>1cm</Height><Width>7cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox60\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>测试对数</Value><Style><FontSize>11pt</FontSize></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>12cm</Top><Left>0.5cm</Left><Height>1cm</Height><Width>7cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    // "  <Textbox Name=\"Textbox59\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>操作员</Value><Style><FontSize>11pt</FontSize></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>13cm</Top><Left>0.5cm</Left><Height>1cm</Height><Width>7cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox58\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>测试日期</Value><Style><FontSize>11pt</FontSize></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>14cm</Top><Left>0.5cm</Left><Height>1cm</Height><Width>7cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox57\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>温度</Value><Style><FontSize>11pt</FontSize></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>15cm</Top><Left>0.5cm</Left><Height>1cm</Height><Width>7cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    // "  <Textbox Name=\"Textbox56\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>湿度</Value><Style><FontSize>11pt</FontSize></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>16cm</Top><Left>0.5cm</Left><Height>1cm</Height><Width>7cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox55\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>规格名称</Value><Style><FontSize>11pt</FontSize></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>17cm</Top><Left>0.5cm</Left><Height>1cm</Height><Width>7cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox54\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>测试设备</Value><Style><FontSize>11pt</FontSize></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>18cm</Top><Left>0.5cm</Left><Height>1cm</Height><Width>7cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox53\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>备注</Value><Style><FontSize>11pt</FontSize></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>19cm</Top><Left>0.5cm</Left><Height>2cm</Height><Width>7cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox522\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>@Ph</Value><Style /></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>9cm</Top><Left>7.8cm</Left><Height>1cm</Height><Width>6.5cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox511\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>@Kh</Value><Style /></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>10cm</Top><Left>7.8cm</Left><Height>1cm</Height><Width>6.5cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox500\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>@Length</Value><Style /></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>11cm</Top><Left>7.8cm</Left><Height>1cm</Height><Width>6.5cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox499\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>@Ds</Value><Style /></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>12cm</Top><Left>7.8cm</Left><Height>1cm</Height><Width>6.5cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    // "  <Textbox Name=\"Textbox488\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>@Operator</Value><Style /></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>13cm</Top><Left>7.8cm</Left><Height>1cm</Height><Width>6.5cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox477\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>@TestDate</Value><Style /></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>14cm</Top><Left>7.8cm</Left><Height>1cm</Height><Width>6.5cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox466\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>@Temperature</Value><Style /></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>15cm</Top><Left>7.8cm</Left><Height>1cm</Height><Width>6.5cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    // "  <Textbox Name=\"Textbox455\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>@Sd</Value><Style /></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>16cm</Top><Left>7.8cm</Left><Height>1cm</Height><Width>6.5cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox444\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>@Spec</Value><Style /></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>17cm</Top><Left>7.8cm</Left><Height>1cm</Height><Width>6.5cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox433\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>@Equipment</Value><Style /></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>18cm</Top><Left>7.8cm</Left><Height>1cm</Height><Width>6.5cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    //"  <Textbox Name=\"Textbox422\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>@Remarks</Value><Style /></TextRun></TextRuns><Style /></Paragraph></Paragraphs>" +
                    //"    <rd:DefaultName>Textbox1</rd:DefaultName><Top>19cm</Top><Left>7.8cm</Left><Height>2cm</Height><Width>6.5cm</Width><Style><Border><Style>None</Style></Border><VerticalAlign>Middle</VerticalAlign><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style>" +
                    //"  </Textbox>" +

                    "</ReportItems><KeepTogether>true</KeepTogether><Height>23.8cm</Height><Width>16cm</Width><Style><Border><Style>None</Style></Border></Style></Rectangle>";
        #endregion

        #region rdlc xml文件

        protected string _docTitle =

            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<Report xmlns=\"http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition\" xmlns:rd=\"http://schemas.microsoft.com/SQLServer/reporting/reportdesigner\">" +

            "<Body>" +
            "<ReportItems>@TEST@Rectangle@ImageFile" +
            "</ReportItems>" +
            "<Style />" +
            "<Height>27cm</Height>" +
            "</Body>" +
            "<Width>16cm</Width>" +
            "<Page>" +

            "<PageFooter>" +
            "<Height>3cm</Height>" +
            "<PrintOnFirstPage>true</PrintOnFirstPage>" +
            "<PrintOnLastPage>true</PrintOnLastPage>" +
            "<ReportItems>@Pages</ReportItems>" +
            "<Style>" +
            "<Border>" +
            "    <Style>None</Style>" +
            "</Border>" +
            "</Style>" +
            "</PageFooter>" +

            "<PageHeight>29.7cm</PageHeight>" +
            "<PageWidth>21cm</PageWidth>" +
            "<LeftMargin>2cm</LeftMargin>" +
            "<RightMargin>2cm</RightMargin>" +
            "<TopMargin>2cm</TopMargin>" +
            "<BottomMargin>2cm</BottomMargin>" +
            "<ColumnSpacing>0.013cm</ColumnSpacing>" +
            "<Style />" +
            "</Page>" +
            "<rd:ReportID>809f16cf-ea78-4469-bf43-965c4afe69d0</rd:ReportID>" +
            "<rd:ReportUnitType>Cm</rd:ReportUnitType>" +
            "</Report>";

        protected string _docTemplate =
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<Report xmlns=\"http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition\" xmlns:rd=\"http://schemas.microsoft.com/SQLServer/reporting/reportdesigner\">" +

            "<DataSources>" +
            "   <DataSource Name=\"DummyDataSource\">" +
            "       <ConnectionProperties>" +
            "           <DataProvider>SQL</DataProvider>" +
            "           <ConnectString />" +
            "       </ConnectionProperties>" +
            "       <rd:DataSourceID>3eecdab9-6b4b-4836-ad62-95e4aee65ea8</rd:DataSourceID>" +
            "   </DataSource>" +
            "</DataSources>" +
            "<DataSets>@DataSets</DataSets>" +

            "<Body>" +
            "<ReportItems>@TEST@Rectangle@titleCount" +
            "</ReportItems>" +
            "<Style />" +
            "<Height>27cm</Height>" +
            "</Body>" +
            "<Width>16cm</Width>" +
            "<Page>" +

            "<PageFooter>" +
            "<Height>3cm</Height>" +
            "<PrintOnFirstPage>true</PrintOnFirstPage>" +
            "<PrintOnLastPage>true</PrintOnLastPage>" +
            "<ReportItems>@Pages</ReportItems>" +
            "<Style>" +
            "<Border>" +
            "    <Style>None</Style>" +
            "</Border>" +
            "</Style>" +
            "</PageFooter>" +

            "<PageHeight>29.7cm</PageHeight>" +
            "<PageWidth>21cm</PageWidth>" +
            "<LeftMargin>2.0cm</LeftMargin>" +
            "<RightMargin>2.0cm</RightMargin>" +
            "<TopMargin>2.0cm</TopMargin>" +
            "<BottomMargin>2.0cm</BottomMargin>" +
            "<ColumnSpacing>0.013cm</ColumnSpacing>" +
            "<Style />" +
            "</Page>" +
            "<rd:ReportID>809f16cf-ea78-4469-bf43-965c4afe69d0</rd:ReportID>" +
            "<rd:ReportUnitType>Cm</rd:ReportUnitType>" +
            "</Report>";

        protected static string ImageFile =
                  " <Image Name=\"Image@TextboxName\">" +
                       "<Source>External</Source> " +
                       "<Value>@ImageNames</Value> " +
                       "<Sizing>Fit</Sizing>" +
                       "<Top>@Postionscm</Top> " +
                       "<Left>1.21005cm</Left> " +
                       "<Height>4cm</Height> " +
                       "<Width>10cm</Width> " +
                       "<ZIndex>1</ZIndex> " +
                       "<Style> " +
                       "  <Border> " +
                       "    <Style>None</Style> " +
                       "  </Border> " +
                       "</Style> " +
                       " </Image>";

        #endregion

        protected List<ReportColoumStyle> _coloumStyle = new List<ReportColoumStyle>();
        protected List<ReportItem> _reportItemPatterns = new List<ReportItem>();
        protected List<ReportItem> _reportImageFilePatterns = new List<ReportItem>();

        protected List<ReportItem> _rectangle2 = new List<ReportItem>();

        protected List<string> _pageFoot = new List<string>();
        protected List<string> _rectangle = new List<string>();

        public DynamicReport()
        {

        }

        public void Dispose()
        {
            _coloumStyle.Clear();
            _reportItemPatterns.Clear();
            _reportImageFilePatterns.Clear();
            _pageFoot.Clear();
            _rectangle.Clear();
            _rectangle2.Clear();
        }

        protected void SetColoumStyle(List<ReportColoumStyle> coloumStyle)
        {
            _coloumStyle = coloumStyle;
        }

        protected void AddImageFile(string file, string name, DataTable data)
        {
            var fields = new StringBuilder();
            var reportItem = new ReportItem();
            var dataSetName = string.Format("picture{0}", _reportImageFilePatterns.Count + 1);
            fields.AppendFormat(
                      "<Field Name=\"{0}\"><DataField>{0}</DataField><rd:TypeName>System.String</rd:TypeName></Field>",
                      "Picture");
            reportItem.DataSetString =
                 reportItem.DataSet
                                  .Replace("@DataSetName", dataSetName)
                                  .Replace("@Fields", fields.ToString());

            reportItem.Data = data;
            reportItem.DataSetName = dataSetName;
            reportItem.PictureString = reportItem.picture.Replace("@DataSetName", dataSetName.ToString())
                                                          .Replace("@title", name.ToString())
                                                          .Replace("@name", file)
                                                          .Replace("@TopPosition", CaculatePlacePostion().ToString());

            _reportImageFilePatterns.Add(reportItem);

        }

        protected void AddPageFoot(string str2)
        {
            Boolean ok = false;
            _pageFoot = new List<string>();
            if (string.IsNullOrEmpty(str2))
            {
                ok = true;
                str2 = "\"Null\"";
            }
            var PageFootPatter = PageFoot.Replace("@DataSetName", "1Page")
                                 .Replace("@str", str2)
                                 .Replace("@show", ok.ToString())
                                 .Replace("@pagefoot", "2page");
            _pageFoot.Add(PageFootPatter);
        }

        protected void AddRectangle(string str1, ReportPackage RP)
        {
            Boolean ok = false;
            _rectangle = new List<string>();
            if (string.IsNullOrEmpty(str1))
            {
                ok = true;
                str1 = "\"Null\"";
            }

            if (RP.TitleName.Contains("&") || RP.TitleName.Contains("<"))
            {
                RP.TitleName = RP.TitleName.Replace("&", "&amp;");
                if (RP.TitleName.Contains("<"))
                {
                    RP.TitleName = RP.TitleName.Replace("<", "&lt;");
                }
            }
           
            var RectanglePatter = Rectangle.Replace("@Rectangle", "1")
                                .Replace("@TitleName", RP.TitleName)
                                .Replace("@Result", RP.Result.ToLower())
                                .Replace("@str", str1)
                                .Replace("@show", ok.ToString());
            _rectangle.Add(RectanglePatter);
        }

        protected void AddData(DataTable dataTable, string tableName, ReportPackage RP, int num)
        {
            var colHeaders = new List<string>();
            var collHeaders = new List<string>();
            if (dataTable != null)
            {
                var coloumNames = new List<string>();
                int i = 0;
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    var headerName = dataColumn.ColumnName;
                    var protertyName = dataColumn.Caption;
                    coloumNames.Add("COL" + i.ToString());
                    colHeaders.Add(protertyName);
                    collHeaders.Add(headerName);
                    dataColumn.ColumnName = "COL" + i.ToString();
                    i++;
                }
                AddReportItemPattern(coloumNames, dataTable, colHeaders.ToArray(), collHeaders.ToArray(), RP, tableName, num);
            }
        }

        protected void AddData2(DataTable dataTable, ReportPackage RP)
        {
            var colHeaders = new List<string>();
            var collHeaders = new List<string>();
            if (dataTable != null)
            {
                var coloumNames = new List<string>();
                long i = 1000000000000000;
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    var headerName = dataColumn.ColumnName;
                    var protertyName = dataColumn.Caption;
                    coloumNames.Add("COL" + i.ToString());
                    colHeaders.Add(protertyName);
                    collHeaders.Add(headerName);
                    dataColumn.ColumnName = "COL" + i.ToString();
                    i++;
                }
                AddReportItemPattern2(coloumNames, dataTable, colHeaders.ToArray(), collHeaders.ToArray(), RP);
            }
        }

        protected void AddReportItemPattern2(List<string> coloumNames, DataTable data, string[] colHeaders = null, string[] collHeaders = null, ReportPackage RP = null)
        {

            var fields = new StringBuilder();
            var coloums = new StringBuilder();
            var tablixHearders = new StringBuilder();
            var tablixCells = new StringBuilder();
            var tablixMembers = new StringBuilder();
            var cell = new StringBuilder();

            int i = 0;
            float ColoumWidth = (float)((15.8) / (Int32.Parse(coloumNames.Count().ToString())));
            int icolindex = 0;
            foreach (var coloumName in coloumNames)
            {
                string headersName = colHeaders[i];
                string tempName = "";
                string tempName2 = "";
                if (headersName.Contains("&") || headersName.Contains("<"))
                {
                    if (headersName.Contains("&") && headersName.Contains("<"))
                    {
                        tempName2 = headersName.Replace("&", "&amp;");
                        tempName = tempName2.Replace("<", "&lt;");

                    }
                    else if (headersName.Contains("&"))
                    {
                        tempName = headersName.Replace("&", "&amp;");

                    }
                    else
                    {
                        tempName = headersName.Replace("<", "&lt;");

                    }
                }
                else
                {
                    tempName = colHeaders[i];
                }

                var coloumWidth = ColoumWidth;

                fields.AppendFormat(
                       "<Field Name=\"{0}\"><DataField>{0}</DataField><rd:TypeName>System.String</rd:TypeName></Field>",
                       coloumName);
                coloums.AppendFormat("<TablixColumn><Width>{0}cm</Width></TablixColumn>", coloumWidth);

                if (colHeaders == null)
                {

                    tablixHearders.AppendFormat("<TablixCell><CellContents>" +
                                                "<Textbox Name=\"Textbox{0}{1}\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph>" +
                                                "<TextRuns><TextRun><Value>{0}</Value><Style><Color>Blue</Color> <FontWeight>Bold</FontWeight></Style></TextRun></TextRuns><Style><TextAlign>Center</TextAlign></Style></Paragraph></Paragraphs>" +
                                                "<rd:DefaultName>Textbox{0}{1}</rd:DefaultName><Style><Border><Style>Solid</Style><Width>1pt</Width></Border><VerticalAlign>Middle</VerticalAlign>" +
                                                "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>", coloumName, Hearders);
                }
                else
                {
                    tablixHearders.AppendFormat("<TablixCell><CellContents>" +
                                                "<Textbox Name=\"Textbox{0}{1}\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph>" +
                                                "<TextRuns><TextRun><Value>{2}</Value><Style><Color>Blue</Color> <FontWeight>Bold</FontWeight></Style></TextRun></TextRuns><Style><TextAlign>Center</TextAlign></Style></Paragraph></Paragraphs>" +
                                                "<rd:DefaultName>Textbox{0}{1}</rd:DefaultName><Style><Border><Color>LightGrey</Color> <Style>Solid</Style>  <Width>0.6pt</Width></Border><VerticalAlign>Middle</VerticalAlign>" +
                                                "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>", coloumName, Hearders, tempName);
                }

                
                tablixCells.AppendFormat("<TablixCell><CellContents>"+
                                            "<Textbox Name=\"{0}{1}1\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether>" +
                                            "<Paragraphs><Paragraph><TextRuns><TextRun><Value>=Fields!{0}.Value</Value><Style/></TextRun></TextRuns><Style><TextAlign>Center</TextAlign></Style></Paragraph></Paragraphs>" +
                                            "<rd:DefaultName>{0}{1}1</rd:DefaultName><Style><Border><Color>LightGrey</Color>  <Style>Solid</Style>  <Width>0.6pt</Width></Border><VerticalAlign>Middle</VerticalAlign>" +
                                            "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>", coloumName, Hearders);

                tablixMembers.AppendFormat("<TablixMember />");

                icolindex++;
                i++;
            };
            for (int j = 1; j < coloumNames.Count; j++)
            { cell.AppendFormat("  <TablixCell />"); }

            var dataSetName = string.Format("TITLE{0}", _rectangle2.Count + 1);
            var reportItem = new ReportItem();
            reportItem.Data = data;
            reportItem.DataSetName = dataSetName;

            reportItem.DataSetString2 =
                reportItem.DataSet
                                 .Replace("@DataSetName", dataSetName)
                                 .Replace("@Fields", fields.ToString());

            reportItem.TablixString2 =
                reportItem.Tablix2
                                .Replace("@Cell", cell.ToString())
                                .Replace("@count", coloumNames.Count.ToString())
                                .Replace("@DataSetName", dataSetName)
                                .Replace("@TablixColumns", coloums.ToString())
                                .Replace("@TablixHeader", tablixHearders.ToString())
                                .Replace("@TablixCells", tablixCells.ToString())
                                .Replace("@TablixMember", tablixMembers.ToString());
                                //.Replace("@TopPosition", (CaculatePlacePostion()).ToString());

            _rectangle2.Add(reportItem);
        }

        protected float CaculatePlacePostion()
        {
            float imageCount = _reportImageFilePatterns.Count * 10f;
            float itemCount = _reportItemPatterns.Count * 3.5f;
            return (itemCount + imageCount + 12.5f);
            //return (itemCount + imageCount + 20.5f);
        }

        string Hearders = "0";
        protected void AddReportItemPattern(List<string> coloumNames, DataTable data, string[] colHeaders = null, string[] collHeaders = null, ReportPackage RP = null, string tableName = null, int num = 0)
        {

            var fields = new StringBuilder();
            var coloums = new StringBuilder();
            var tablixHearders = new StringBuilder();
            var tablixCells = new StringBuilder();
            var tablixMembers = new StringBuilder();
            var cell = new StringBuilder();
            var currentNamePrefix = (_reportItemPatterns.Count + 1).ToString();
            Hearders = currentNamePrefix + num.ToString();


            int i = 0;
            float ColoumWidth = (float)((15.8) / (Int32.Parse(coloumNames.Count().ToString())));
            int icolindex = 0;
            foreach (var coloumName in coloumNames)
            {
                string headersName = colHeaders[i];
                string tempName = "";
                string tempName2 = "";
                if (headersName.Contains("&") || headersName.Contains("<"))
                {
                    if (headersName.Contains("&") && headersName.Contains("<"))
                    {
                        tempName2 = headersName.Replace("&", "&amp;");
                        tempName = tempName2.Replace("<", "&lt;");

                    }
                    else if (headersName.Contains("&"))
                    {
                        tempName = headersName.Replace("&", "&amp;");

                    }
                    else
                    {
                        tempName = headersName.Replace("<", "&lt;");

                    }
                }
                else
                {
                    tempName = colHeaders[i];
                }

                var coloumWidth = ColoumWidth;

                fields.AppendFormat(
                       "<Field Name=\"{0}\"><DataField>{0}</DataField><rd:TypeName>System.String</rd:TypeName></Field>",
                       coloumName);
                coloums.AppendFormat("<TablixColumn><Width>{0}cm</Width></TablixColumn>", coloumWidth);

                if (colHeaders == null)
                {

                    tablixHearders.AppendFormat("<TablixCell><CellContents>" +
                                                "<Textbox Name=\"Textbox{0}{1}\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph>" +
                                                "<TextRuns><TextRun><Value>{0}</Value><Style><Color>Blue</Color> <FontWeight>Bold</FontWeight></Style></TextRun></TextRuns><Style><TextAlign>Center</TextAlign></Style></Paragraph></Paragraphs>" +
                                                "<rd:DefaultName>Textbox{0}{1}</rd:DefaultName><Style><Border><Style>Solid</Style><Width>1pt</Width></Border><VerticalAlign>Middle</VerticalAlign>" +
                                                "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>", coloumName, Hearders);
                }
                else
                {
                    if (icolindex == 0)
                    {
                        tablixHearders.AppendFormat("<TablixCell><CellContents>" +
                                                   "<Textbox Name=\"Textbox{0}{1}\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph>" +
                                                   "<TextRuns><TextRun><Value>{2}</Value><Style><Color>Blue</Color> <FontWeight>Bold</FontWeight></Style></TextRun></TextRuns><Style><TextAlign>Center</TextAlign></Style></Paragraph></Paragraphs>" +
                                                   "<rd:DefaultName>Textbox{0}{1}</rd:DefaultName><Style><Border><Color>Black</Color><Style>Solid</Style><Width>0.5pt</Width></Border><LeftBorder><Color>Black</Color><Style>Solid</Style><Width>1.2pt</Width></LeftBorder> <BottomBorder><Color>Black</Color><Style>Solid</Style><Width>1.2pt</Width> </BottomBorder><VerticalAlign>Middle</VerticalAlign>" +
                                                   "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>", coloumName, Hearders, tempName);
                    }
                    else if (icolindex == coloumNames.Count - 1)
                    {
                        tablixHearders.AppendFormat("<TablixCell><CellContents>" +
                                                   "<Textbox Name=\"Textbox{0}{1}\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph>" +
                                                   "<TextRuns><TextRun><Value>{2}</Value><Style><Color>Blue</Color> <FontWeight>Bold</FontWeight></Style></TextRun></TextRuns><Style><TextAlign>Center</TextAlign></Style></Paragraph></Paragraphs>" +
                                                   "<rd:DefaultName>Textbox{0}{1}</rd:DefaultName><Style><Border><Color>Black</Color><Style>Solid</Style><Width>0.5pt</Width></Border><RightBorder><Color>Black</Color><Style>Solid</Style><Width>1.2pt</Width></RightBorder> <BottomBorder><Color>Black</Color><Style>Solid</Style><Width>1.2pt</Width> </BottomBorder><VerticalAlign>Middle</VerticalAlign>" +
                                                   "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>", coloumName, Hearders, tempName);
                    }
                    else
                    {
                        tablixHearders.AppendFormat("<TablixCell><CellContents>" +
                                                   "<Textbox Name=\"Textbox{0}{1}\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph>" +
                                                   "<TextRuns><TextRun><Value>{2}</Value><Style><Color>Blue</Color> <FontWeight>Bold</FontWeight></Style></TextRun></TextRuns><Style><TextAlign>Center</TextAlign></Style></Paragraph></Paragraphs>" +
                                                   "<rd:DefaultName>Textbox{0}{1}</rd:DefaultName><Style><Border><Color>Black</Color><Style>None</Style></Border> <TopBorder><Color>Black</Color><Style>Solid</Style><Width>0.5pt</Width></TopBorder><LeftBorder><Color>Black</Color><Style>Solid</Style><Width>0.5pt</Width></LeftBorder><BottomBorder><Color>Black</Color><Style>Solid</Style><Width>1.2pt</Width></BottomBorder><VerticalAlign>Middle</VerticalAlign>" +
                                                   "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>", coloumName, Hearders, tempName);
                    }
                }

                if (collHeaders[i].ToUpper().Equals("结论"))
                {
                    if (icolindex == 0)
                    {
                        tablixCells.AppendFormat("<TablixCell><CellContents><Textbox Name=\"{0}{1}1\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether>" +
                                                     "<Paragraphs><Paragraph><TextRuns><TextRun><Value>=Fields!{0}.Value</Value><Style><Color>=IIF(TRIM(Fields!{0}.Value.ToUpper())=\"P\",\"Black\",\"Red\")</Color><FontWeight>Bold</FontWeight></Style></TextRun></TextRuns><Style><TextAlign>Center</TextAlign></Style></Paragraph></Paragraphs>" +
                                                     "<rd:DefaultName>{0}{1}1</rd:DefaultName><Style><Border><Color>LightGrey</Color><Style>Solid</Style><Width>0.6pt</Width></Border> <LeftBorder><Color>Black</Color><Style>Solid</Style><Width>1.2pt</Width></LeftBorder><VerticalAlign>Middle</VerticalAlign>" +
                                                     "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>", coloumName, Hearders);
                    }
                    else if (icolindex == coloumNames.Count - 1)
                    {
                        tablixCells.AppendFormat("<TablixCell><CellContents><Textbox Name=\"{0}{1}1\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether>" +
                                                    "<Paragraphs><Paragraph><TextRuns><TextRun><Value>=Fields!{0}.Value</Value><Style><Color>=IIF(TRIM(Fields!{0}.Value.ToUpper())=\"P\",\"Black\",\"Red\")</Color><FontWeight>Bold</FontWeight></Style></TextRun></TextRuns><Style><TextAlign>Center</TextAlign></Style></Paragraph></Paragraphs>" +
                                                    "<rd:DefaultName>{0}{1}1</rd:DefaultName><Style><Border><Color>LightGrey</Color><Style>Solid</Style><Width>0.6pt</Width></Border> <RightBorder><Color>Black</Color><Style>Solid</Style><Width>1.2pt</Width></RightBorder><VerticalAlign>Middle</VerticalAlign>" +
                                                    "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>", coloumName, Hearders);

                    }
                    else
                    {
                        tablixCells.AppendFormat("<TablixCell><CellContents><Textbox Name=\"{0}{1}1\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether>" +
                                                     "<Paragraphs><Paragraph><TextRuns><TextRun><Value>=Fields!{0}.Value</Value><Style><Color>=IIF(TRIM(Fields!{0}.Value.ToUpper())=\"P\",\"Black\",\"Red\")</Color><FontWeight>Bold</FontWeight></Style></TextRun></TextRuns><Style><TextAlign>Center</TextAlign></Style></Paragraph></Paragraphs>" +
                                                     "<rd:DefaultName>{0}{1}1</rd:DefaultName><Style><Border><Color>LightGrey</Color><Style>Solid</Style><Width>0.6pt</Width></Border> <VerticalAlign>Middle</VerticalAlign>" +
                                                     "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>", coloumName, Hearders);
                    }

                }
                else
                {
                    if (icolindex == 0)
                    {
                        tablixCells.AppendFormat("<TablixCell><CellContents><Textbox Name=\"{0}{1}1\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether>" +
                                                  "<Paragraphs><Paragraph><TextRuns><TextRun><Value>=Fields!{0}.Value</Value><Style/></TextRun></TextRuns><Style><TextAlign>Center</TextAlign></Style></Paragraph></Paragraphs>" +
                                                  "<rd:DefaultName>{0}{1}1</rd:DefaultName><Style><Border><Color>LightGrey</Color><Style>Solid</Style><Width>0.6pt</Width></Border><LeftBorder><Color>Black</Color><Style>Solid</Style><Width>1.2pt</Width></LeftBorder><VerticalAlign>Middle</VerticalAlign>" +
                                                  "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>", coloumName, Hearders);
                    }
                    else if (icolindex == coloumNames.Count - 1)
                    {
                        tablixCells.AppendFormat("<TablixCell><CellContents><Textbox Name=\"{0}{1}1\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether>" +
                                                  "<Paragraphs><Paragraph><TextRuns><TextRun><Value>=Fields!{0}.Value</Value><Style/></TextRun></TextRuns><Style><TextAlign>Center</TextAlign></Style></Paragraph></Paragraphs>" +
                                                  "<rd:DefaultName>{0}{1}1</rd:DefaultName><Style><Border><Color>LightGrey</Color><Style>Solid</Style><Width>0.6pt</Width></Border><RightBorder> <Color>Black</Color><Style>Solid</Style><Width>1.2pt</Width></RightBorder><VerticalAlign>Middle</VerticalAlign>" +
                                                  "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>", coloumName, Hearders);
                    }
                    else
                    {
                        tablixCells.AppendFormat("<TablixCell><CellContents><Textbox Name=\"{0}{1}1\"><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether>" +
                                                  "<Paragraphs><Paragraph><TextRuns><TextRun><Value>=Fields!{0}.Value</Value><Style/></TextRun></TextRuns><Style><TextAlign>Center</TextAlign></Style></Paragraph></Paragraphs>" +
                                                  "<rd:DefaultName>{0}{1}1</rd:DefaultName><Style><Border><Color>LightGrey</Color><Style>Solid</Style><Width>0.6pt</Width></Border><VerticalAlign>Middle</VerticalAlign>" +
                                                  "<PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>", coloumName, Hearders);
                    }


                }

                tablixMembers.AppendFormat("<TablixMember />");

                icolindex++;
                i++;
            };
            for (int j = 1; j < coloumNames.Count; j++)
            { cell.AppendFormat("  <TablixCell />"); }


            var dataSetName = string.Format("Data{0}", _reportItemPatterns.Count + 1);
            var name = string.Format("Name{0}", _reportItemPatterns.Count + 1);
            var reportItem = new ReportItem();
            reportItem.Data = data;
            reportItem.DataSetName = dataSetName;

            reportItem.DataSetString =
                reportItem.DataSet
                                 .Replace("@DataSetName", dataSetName)
                                 .Replace("@Fields", fields.ToString());

            reportItem.TablixString =
                reportItem.Tablix
                                .Replace("@Cell", cell.ToString())
                                .Replace("@count", coloumNames.Count.ToString())
                                .Replace("@title", tableName.ToString())
                                .Replace("@DataSetName", dataSetName)
                                .Replace("@TablixColumns", coloums.ToString())
                                .Replace("@TablixHeader", tablixHearders.ToString())
                                .Replace("@TablixCells", tablixCells.ToString())
                                .Replace("@TablixMember", tablixMembers.ToString())
                                .Replace("@TopPosition", (CaculatePlacePostion()).ToString());

            _reportItemPatterns.Add(reportItem);
        }

        protected void ShowReport(string FileType, string FilePath)
        {
            ReportViewer _report = new ReportViewer();
            var dataSetsString = new StringBuilder();
            var tablixString = new StringBuilder();

            //var dataSetsString2 = new StringBuilder();
            var tablixString2 = new StringBuilder();

            var pageFootString = new StringBuilder();
            var rectangleString = new StringBuilder();
            var titleCount = new StringBuilder();
            foreach (var reportItemPattern in _reportItemPatterns)
            {
                dataSetsString.Append(reportItemPattern.DataSetString);
                tablixString.Append(reportItemPattern.TablixString);
                //rectangleString.Append(reportItemPattern.RectangleString);
            }
            titleCount.Append(tablixString);
            var reportImageFileString = new StringBuilder();
            foreach (var reportImageFilePatterns in _reportImageFilePatterns)
            {
                reportImageFileString.Append(reportImageFilePatterns.PictureString);
                dataSetsString.Append(reportImageFilePatterns.DataSetString);
            }
            titleCount.Append(reportImageFileString);
            var reportPageFootString = new StringBuilder();
            foreach (var pageFootPatterns in _pageFoot)
            {
                reportPageFootString.Append(pageFootPatterns);
            }

            var reportRectangleString = new StringBuilder();
            foreach (var reportItemPattern in _rectangle2)
            {
                dataSetsString.Append(reportItemPattern.DataSetString2);
                tablixString2.Append(reportItemPattern.TablixString2);
            }
            reportRectangleString.Append(tablixString2);
            var reportRectangleString3 = new StringBuilder();
            foreach (var rectanglePatterns in _rectangle)
            {
                reportRectangleString3.Append(rectanglePatterns);
            }
            if (_reportItemPatterns.Count > 0 || _reportImageFilePatterns.Count > 0)
            {

                _docTemplate = _docTemplate
                            .Replace("@TEST", reportRectangleString3.ToString())
                            .Replace("@Rectangle", reportRectangleString.ToString())
                            .Replace("@Pages", reportPageFootString.ToString())
                            .Replace("@DataSets", dataSetsString.ToString())
                            .Replace("@titleCount", titleCount.ToString());

                var doc = new XmlDocument();
                doc.LoadXml(_docTemplate);

                Stream stream = GetRdlcStream(doc);

                //加载报表定义
                _report.LocalReport.EnableExternalImages = true;
                _report.LocalReport.LoadReportDefinition(stream);
                _report.LocalReport.DataSources.Clear();

                foreach (var reportImageFilePatterns in _rectangle2)
                {
                    ReportDataSource rd = new ReportDataSource(reportImageFilePatterns.DataSetName + "Data", reportImageFilePatterns.Data);
                    _report.LocalReport.DataSources.Add(rd);
                }

                foreach (var reportItemPattern in _reportItemPatterns)
                {
                    ReportDataSource rds = new ReportDataSource(reportItemPattern.DataSetName + "Data", reportItemPattern.Data);
                    _report.LocalReport.DataSources.Add(rds);
                }

                foreach (var reportImageFilePatterns in _reportImageFilePatterns)
                {
                    ReportDataSource rd = new ReportDataSource(reportImageFilePatterns.DataSetName + "Data", reportImageFilePatterns.Data);
                    _report.LocalReport.DataSources.Add(rd);
                }
            }
            else
            {

                _docTitle = _docTitle
                            .Replace("@TEST", reportRectangleString3.ToString())
                            .Replace("@Rectangle", reportRectangleString.ToString())
                            .Replace("@Pages", reportPageFootString.ToString());
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_docTitle);
                Stream stream = GetRdlcStream(doc);
                _report.LocalReport.EnableExternalImages = true;
                _report.LocalReport.LoadReportDefinition(stream);

            }
            _reportItemPatterns = new List<ReportItem>();
            _reportImageFilePatterns = new List<ReportItem>();
            _report.LocalReport.Refresh();

            if (FileType.ToUpper().Equals("PDF") || FileType.ToUpper().Equals("WORD") || FileType.ToUpper().Equals("EXCEL"))
            {

                byte[] bytes = _report.LocalReport.Render(FileType);

                if (FileType.ToUpper().Equals("PDF"))
                {
                    FilePath += ".pdf";
                }
                if (FileType.ToUpper().Equals("WORD"))
                {
                    FilePath += ".doc";
                }
                if (FileType.ToUpper().Equals("EXCEL"))
                {
                    FilePath += ".xls";
                }

                string path = @"D:\"+ "wenjian";
                ReportInterface.RdlcEmfUtils rdlc = new ReportInterface.RdlcEmfUtils(_report.LocalReport, path);

                using (System.IO.FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Dispose();
                }
                _report.LocalReport.Dispose();
                _report.Dispose();
                _report = null;
                bytes = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();

            }
            else
            {
                MessageBox.Show("导出的文件格式应为 PDF、Excel、Word 中的一种");
            }

        }

        protected Stream GetRdlcStream(XmlDocument xmlDoc)
        {
            Stream ms = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(XmlDocument));
            serializer.Serialize(ms, xmlDoc);
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// 显示报表
        /// </summary>
        /// <param name="RP">报表数据</param>
        /// <param name="CustomerLg">客户Logo</param>
        /// <param name="CompanyLg">公司Logo</param>
        /// <param name="FileType">文件格式</param>
        /// <param name="FilePath">文件夹</param>
        public void ShowReport(ReportPackage RP, Image CustomerLg, Image CompanyLg, string FileType, string FilePath)
        {
          
            bool flag = false;
            bool result = false;
            string testResult = "合格";
            List<ReportColoumStyle> lstCol = null;
            foreach (ReportItems report in RP.ReportItems)
            {
                int num = 0;
                if (report.dt != null)
                {
                    foreach (DataTableItem item in report.dt)
                    {
                        lstCol = new List<ReportColoumStyle>();
                        foreach (DataColumn dc in item.dt.Columns)
                        {
                            if (!result && dc.ColumnName.Equals("结论"))
                            {
                                flag = true;
                            }
                            lstCol.Add(new ReportColoumStyle() { ColoumName = dc.ColumnName, ColoumWidth = 4F });
                        }
                        SetColoumStyle(lstCol);

                        num++;
                        DataTable dtVal = item.dt.Clone();//克隆
                        foreach (DataRow dr in item.dt.Rows)
                        {
                            if (flag && Convert.ToString(dr["结论"]).ToUpper().Equals("FAIL"))
                            {
                                result = true;
                                flag = false;
                                testResult = "不合格";
                            }
                            dtVal.ImportRow(dr); //导入
                        }

                        //  加入数据
                        string name = item.name.ToString();
                        string tempName = "";
                        string tempName2 = "";
                        if (name.Contains("&") || name.Contains("<"))
                        {
                            if (name.Contains("&") && name.Contains("<"))
                            {
                                tempName2 = name.Replace("&", "&amp;");
                                tempName = tempName2.Replace("<", "&lt;");
                                AddData(dtVal, tempName, RP, num);
                            }
                            else if (name.Contains("&"))
                            {
                                tempName = name.Replace("&", "&amp;");
                                AddData(dtVal, tempName, RP, num);
                            }
                            else
                            {
                                tempName = name.Replace("<", "&lt;");
                                AddData(dtVal, tempName, RP, num);
                            }
                        }
                        else
                        {
                            AddData(dtVal, item.name.ToString(), RP, num);
                        }
                    }
                }
                if (report.image != null)
                {
                    foreach (ImageItem items in report.image)
                    {
                        DataTable dt = new DataTable();
                        dt.Clear();
                        dt.Columns.Add("Picture");
                        dt.Rows.Add(new object[] { Convert.ToString(" ") });
                        Image ig = items.image;
                        string path = Path.GetTempFileName() + ".png";
                        ig.Save(path);
                        ig.Dispose();
                        string file = "file:///" + path;
                        string name = items.name.ToString();
                        string tempName = "";
                        string tempName2 = "";
                        if (name.Contains("&") || name.Contains("<"))
                        {
                            if (name.Contains("&") && name.Contains("<"))
                            {
                                tempName2 = name.Replace("&", "&amp;");
                                tempName = tempName2.Replace("<", "&lt;");
                                AddImageFile(file, tempName.ToString(), dt);
                            }
                            else if (name.Contains("&"))
                            {
                                tempName = name.Replace("&", "&amp;");
                                AddImageFile(file, tempName.ToString(), dt);
                            }
                            else
                            {
                                tempName = name.Replace("<", "&lt;");
                                AddImageFile(file, tempName.ToString(), dt);
                            }
                        }
                        else
                        {
                            AddImageFile(file, items.name.ToString(), dt);
                        }
                    }
                }
            }

            RP.Result = testResult;
            
            string str1 = "";
            string str2 = "";
            if (CustomerLg != null)
            {
                string path = Path.GetTempFileName() + ".png";
                CustomerLg.Save(path);
                str1 = "file:///" + path;
            }
            if (CompanyLg != null)
            {
                string path = Path.GetTempFileName() + ".png";
                CompanyLg.Save(path);
                str2 = "file:///" + path;
            }

            lstCol = new List<ReportColoumStyle>();
            foreach (DataColumn dc in RP.dt.Columns)
            {
                lstCol.Add(new ReportColoumStyle() { ColoumName = dc.ColumnName, ColoumWidth = 4F });
            }
            SetColoumStyle(lstCol);
            AddRectangle(str1, RP);
            AddData2(RP.dt, RP);
            AddPageFoot(str2);

            ShowReport(FileType, FilePath);
            Hearders = "0";
            RP.Dispose();
            System.GC.Collect();
            GC.WaitForPendingFinalizers();
        }

    }
}

<template>
  <section v-loading="loading" class="yh-body">
        <div class="yh-list-header flex-between">
             <div>
                <el-date-picker
                    v-model="dateArr"
                    value-format="yyyy-MM-dd"
                    :editable='false'
                    @change="rangeTime"
                    type="daterange"
                    range-separator="至"
                    start-placeholder="开始日期"
                    end-placeholder="结束日期">
                </el-date-picker>
                <el-button @click="query">查询</el-button>                
            </div>
        </div>
        <!-- 表格 -->
       <div class="container">
         <div id="chartPie" style="height:308px"></div>
       </div>
  </section>
</template>

<script>
import listMixins from '@/mixins/list'
export default {
  mixins:[listMixins],
  data() {
    return {
      chartPie:null,
      loading:true,
      dateArr:'',
      params: {//只需要业务参数
        keyWord:'',
        startTime: '',
        endTime: '',
        skipCount:1,
        maxResultCount:100
      },
      option:{
        title : {
            text: '流量统计',
            subtext: '',
            x:'center'
        },
        tooltip : {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        legend: {
            orient : 'vertical',
            x : 'left',
            data:['付费用户数','新增用户数','新增医生数','新增付费用户数','复购用户数']
        },
        toolbox: {
            show : true,
            feature : {
                mark : {
                  show: true
                },
                magicType : {
                    show: true, 
                    type: ['pie', 'funnel'],
                },
                restore : {show:false,title : '刷新',},
                dataView : {
                show : true,
                title : '数据视图',
                readOnly: true,
                lang: ['数据视图', '关闭'],
                /*optionToContent: function(opt) {
                      var data = opt.series[0].data;
                      var textbox = '<div style="margin: 0px 0px 8px; padding: 4px 6px; overflow: auto; width: 100%; height: 228px;border:1px solid #f5f5f5;">'
                                   + '<p>总用户数：'+allSum+'</p>';
                      for (var i = 0; i < data.length; i++) {
                          textbox += '<p>'+data[i].name+'：'+data[i].value+'</p>';
                      }
                      textbox += '</div>';
                      return textbox;
                  }*/
            },
                saveAsImage : {show: true,title:'下载'}
            }
        },
        calculable : true,
        series : [
            {
                name:'流量分析',
                type:'pie',
                radius : '55%',
                center: ['50%', '60%'],
                data:[
                    {value:335, name:'付费用户数'},
                    {value:310, name:'新增用户数'},
                    {value:234, name:'新增医生数'},
                    {value:135, name:'新增付费用户数'},
                    {value:1548, name:'复购用户数'}
                ]
            }
        ]
    }
    }
  },
  methods:{
    query(){
      this.getData(this.params);
    },
    getData(params){
      this.loading=true;
      this.$http
          .post(this.$api.flowList,params)
          .then(res => {
              this.loading = false;
              if (res.result.code == '200') {
                  const initData = [res.result.item.paidUser,res.result.item.newUser,res.result.item.newDoctor,res.result.item.newPaidUser,res.result.item.newRepurchaseCount];
                  this.option.title.subtext="总用户数"+res.result.item.totalUser;
                  this.option.series[0].data=this.option.series[0].data.map(function(data,index){
                    data.value=initData[index];
                    return data;
                  });
                  this.chartPie = echarts.init(document.getElementById("chartPie"));
                  this.chartPie.setOption(this.option);
              }
          })
          .catch(error => {
              this.loading = false;
          });
    }
  },
  created(){
    this.getRangeTime();
    this.getData(this.params);
  }
};
</script>
<style scoped>
.container{width:600px;margin:30px auto 0px auto;}
</style>

<template>
  <div class="container">
     <div id="chartPie" style="height:308px"></div>
  </div>
</template>

<script>
export default {
  data() {
    return {
    	option:{
		    title : {
		        text: '流量统计',
		        subtext: '总用户数',
		        x:'center'
		    },
		    tooltip : {
		        trigger: 'item',
		        formatter: "{a} <br/>{b} : {c} ({d}%)"
		    },
		    legend: {
		        orient : 'vertical',
		        x : 'left',
		        data:['付费用户数10','新增用户数','新增医生数','新增付费用户数','复购用户数']
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
  mounted() {
    //this.chartPie.resize();
    this.chartPie = echarts.init(document.getElementById("chartPie"));
    this.chartPie.setOption(this.option);
  },
 
};
</script>

<style lang="scss" scoped>
.container{width:600px;margin:0 auto;}
</style>


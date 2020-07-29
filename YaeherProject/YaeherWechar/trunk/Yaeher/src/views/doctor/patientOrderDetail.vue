<template>
    <div class="patient-order-detail">
        <mt-header fixed title="咨询记录详情详情">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>               
        </mt-header>
        <div class="content">
			<div class="condition">
				<ul class="paList">
					<li class="border-none">
						姓名：<span>小范范</span>
					</li>
					<li>
						性别：<span>女</span>
					</li>
					<li>
						年龄：<span>1岁</span>
					</li>
					<li>
						病种：<span>儿童常见病</span>
					</li>
					<li>
						所在地：<span>深圳</span>
					</li>
					<li>
						药物过敏：<span>无</span>
					</li>
					<li>
						病情描述：
					</li>
					<li>
						<span style="line-height: 20px;padding: 0 10px; display: inline-block;">精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎</span>
                        <p style="padding: 5px 10px 0 10px;">08月11日13:48</p>
                    </li>
					<li>
						追问：
					</li>
					<li>
						<span style="line-height: 20px;padding: 0 10px; display: inline-block;">精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎</span>
						<p style="padding: 5px 10px 0 10px;">08月11日13:48</p>
					</li>
                    <li>
						我的回答：
					</li>
					<li>
						<span style="line-height: 20px;padding: 0 10px; display: inline-block;">精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎，这是医生的解答。精神好，一般不考虑脑炎</span>
						<p style="padding: 5px 10px 0 10px;">08月11日13:48</p>
					</li>
                    <li>
						患者评分：<span class="star4"></span>
					</li>
                    <li>
						患者评价：
					</li>
                    <li>
						<span style="line-height: 20px;padding: 0 10px; display: inline-block;">医生回答较好</span>
						<p style="padding: 5px 10px 0 10px;">08月11日13:48</p>
					</li>
                    <li>
						质控评分：<span class="star5"></span>
					</li>
                    <li>
						质控评价：
					</li>
                    <li>
						<span style="line-height: 20px;padding: 0 10px; display: inline-block;">医生回答中肯</span>
						<p style="padding: 5px 10px 0 10px;">08月11日13:48</p>
					</li>
				</ul>
			</div>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
export default {
    data () {
        return {
            operaShow: false
        }
    },
    methods: {
        operationShow() {
            this.operaShow = !this.operaShow
        },
        collectOrder() {
            Toast({
                message: '收藏成功',
                duration: 1000
            });
            this.operaShow = false
        }
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.patient-order-detail {
	padding: 42px 0 0;
	.condition {margin: 10px 0 10px; background: $color-wfont; font-size: $font-l; color: #999;}
	.conditionP {padding-bottom: 10px;}
	.condition span {color: $color-afont;}
	.condition .paList li {border-top: 1px dashed #eee; padding: 10px 0;}
	.condition .paList .border-none {border: none;}
}

</style>
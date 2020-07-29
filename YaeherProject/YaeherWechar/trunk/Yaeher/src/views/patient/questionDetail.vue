<template>
    <div class="question-detail">
        <div class="content">
            <div class="doctorDetailList">
				<!-- <ul class="mui-table-view">
					<li class="mui-table-view-cell question-border">
				        <router-link class="mui-navigate-right articleList" :to="{path: '/doctor-detail-patient', query: {id: questionDetail.doctorId}}">
				        	<img class="img-avater" :src="questionDetail.userImage" align="articleList" />
				        	<div class="articleCase">
				        		<p class="blod">{{questionDetail.doctorName}}</p>
				        		<p class="font-six"> 
				        			<span>{{questionDetail.doctorTitle}}</span> 
				        			<span>{{questionDetail.clinicName}}</span> 
				        			<span>{{questionDetail.hospital}}</span>
								</p>
                                <i class="mint-cell-allow-right"></i>
				        	</div>
				        </router-link>
				    </li>
				</ul> -->
				<ul class="mui-table-view">
					<li class="mui-table-view-cell lookP">
						<!-- <span class="lookHint">{{questionDetail.title}}</span> -->
						<div class="border-dashed lookDiv">
							<span class="wen-ti">问题：</span>{{questionDetail.descriptionTiltle}}
						</div>
						<div class="questionContent lookDiv">
							<span class="hui-da">回答：</span>
							<div v-html="questionDetail.answer"></div>
						</div>
						<div class="font-six lookDiv">*本咨询内容由用户授权同意公开，为保护用户隐私并方便阅读，提问以及回复可能有所删减</div>
						<!-- <div>
							<span>{{questionDetail.doctorName}}</span> <span>{{questionDetail.doctorTitle}}</span> <span>{{questionDetail.hospital}}</span>
						</div> -->
						<div class="lookDiv">
							阅读：{{questionDetail.readTotal}}&nbsp;&nbsp; &nbsp;&nbsp;点赞：{{questionDetail.upvoteTotal}}
						</div>
						<div class="clickOk" :class="{alreadyOk: questionDetail.hasParise}">
							<div @click="clickCollect">有帮助（{{questionDetail.upvoteTotal}}）</div>
						</div>
					</li>
					
				</ul>
			</div>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui'
export default {
    data () {
        return {
			id: 0,
			questionDetail: {}
        }
	},
    methods: {
		getQuestionDetail() { // 获取问答详情
			const id = this.id
            this.instance.questionReleaseById({
				id
            })
                .then((response) => {
					this.questionDetail = response.data.result.item
                })
                .catch((error) => {
                }) 
		},
		clickCollect() { //点赞
			const id = this.id
			if(this.questionDetail.hasParise) return
            this.instance.questionReleasepraise({
				id
            })
                .then((response) => {
					Toast('点赞成功')
					this.getQuestionDetail()
                })
                .catch((error) => {
                }) 
		},
        getUserInfo() { // 请求个人信息
            this.instance.patientInfo({
            })
                .then((response) => {
                    const userId = response.data.result.item.id
                    window.sessionStorage.setItem('userId', userId)

                })
                .catch((error) => {
                }) 
        }
    },
    mounted () {
		this.getQuestionDetail()
		this.getUserInfo()
    },
    created () {
		this.id = this.$route.query.id
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.question-detail {
	.doctorDetailList .articleList .articleHint{top: 43px; right: 35px;}
	.doctorDetailList .question-border {border-bottom: 1px solid #f0f0f0;}
	.articleList {overflow: hidden; display: flex; position: relative;}
	.articleList img {float: left; width: 60px; height: 60px; border-radius: 8px;}
	.articleList div {float: left; flex: 1; padding-left: 10px;}
	.articleList .blod {color: $color-afont;}
	.articleList .hintRed {color: $color-red;}
	.lookHint {color: $color-wfont; font-size: $font-l; background: $default-color; display: block; padding: 3px 5px 1px; line-height: 16px;  border-radius: 9px; display: inline-block;}
	.lookP .lookDiv {color: $color-afont; padding: 10px 0;}
	.clickOk {display: flex; justify-content: center; align-items: center; padding: 20px 0;}
	.clickOk div {padding: 5px 10px; border: 1px solid $color-bfont; color: $color-bfont; border-radius: 3px;}
	.question-border .img-avater {width: 40px; height: 40px; border-radius: 50%;}
	.lookP .font-six {color: $color-bfont; font-size: 12px;}
	.lookP .border-dashed {border-bottom: 1px dashed #ccc;}
	.wen-ti {color: $color-red;}
	.hui-da {color: $color-green;}
	.alreadyOk div {border: 1px solid $default-color; color: $default-color;}
}

</style>
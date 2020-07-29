<template>
    <div class="article-detail padding-top">
        <mt-header fixed :title="paperTiltle">
            <a v-if="rShow != 'no'" @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="articleCase">
                <h1>{{articleDetail.paperTiltle}}</h1>
                <div class="paperContent" v-html="articleDetail.paperContent"></div>
            </div>
            <!-- <div v-if="consultID != 0" class="okCase">
                <router-link  :to="{path: '/order-look', query: {id: consultID}}" tag="div">
                    <mt-button type="primary" class="mint-button--large">查看咨询详情</mt-button>
                </router-link>
            </div> -->
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            id: 0,
            rShow: '',
            look: '',
            consultID: 0,
            paperTiltle: '',
            articleDetail: {}
        }
	},
	methods: {
		getArticleDetail () {
			const id = this.id
            this.instance.doctorPaperById({
				id
            })
                .then((response) => {
                    this.articleDetail = response.data.result.item
                    this.paperTiltle = this.articleDetail.paperTiltle
                })
                .catch((error) => {
                }) 
        },
        getArticleDetail1 () {
			const id = this.id
            this.instance.releaseManageById({
				id
            })
                .then((response) => {
                    this.articleDetail = response.data.result.item
                    this.paperTiltle = this.articleDetail.paperTiltle
                })
                .catch((error) => {
                }) 
		}
	},
	mounted () {
        if (this.look === 'no') {
            this.getArticleDetail1()
        } else {
            this.getArticleDetail()
        }
		
	},
	created () {
        this.id = parseInt(this.$route.query.id)
        this.rShow = this.$route.query.rShow
        this.look = this.$route.query.look
        let consultID1 = this.$route.query.consultID
        if (consultID1) {
            this.consultID = parseInt(consultID1)
        }
        
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";
.article-detail{
    background-color: $color-wfont;
    .articleCase {
        padding: 20px 16px 12px;
        background: #fff;
        overflow: auto;
        -webkit-overflow-scrolling: touch;
        .paperContent {
            background: #fff;
            img {
                width: 100%;
            }
        }
        h1 {
            padding: 0 0 14px;
            font-size: 22px;
            line-height: 30px;
            font-weight: 400;
        }
    }
    
}

</style>
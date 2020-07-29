<template>
    <div class="guide-detail padding-top">
        <mt-header fixed title="指南详情">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content articleCase">
            <h1>{{articleDeatil.rulesTitle}}</h1>
            <div class="paperContent" v-html="articleDeatil.rulesContent"></div>
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            id: 0,
            articleDeatil: {}
        }
    },
    methods: {
        getArticleDetailt() {
            const id = this.id
            this.instance.doctorRulesById({
                id
            })
                .then((response) => {
                    this.articleDeatil = response.data.result.item
                })
                .catch((error) => {
                }) 
		}
    },
    mounted () {
        this.getArticleDetailt()
    },
	created () {
      this.id = parseInt(this.$route.query.id)
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.guide-detail{
    background-color: $color-wfont;
    .articleCase {
        padding: 20px 16px 12px;
        background: #fff;
        overflow: auto;
        -webkit-overflow-scrolling: touch;
        .paperContent {
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
<template>
    <div class="article">
        <ul class="mui-table-view guide-margin">
            <li class="mui-table-view-cell">
                <router-link :to="{path: '/guide-detail', query: {controlShow}}" class="mui-navigate-right articleList">
                    <div style="width: 60px; height: 60px; padding-left: 0;"><img src="../../assets/image/article1.jpg" align="articleList" /></div>
                    <div class="articleCase">
                        <p class="blod">评分规则
                            <span class="red-hint">待审核</span>
                        </p>
                        <p class="oneLine">这是评分规则简单介绍</p>
                        <p>2018-08-21</p>
                        <i class="mint-cell-allow-right"></i>
                    </div>
                </router-link>
            </li>
            <li class="mui-table-view-cell">
                <router-link :to="{path: '/guide-detail', query: {controlShow}}" class="mui-navigate-right articleList">
                    <div style="width: 60px; height: 60px; padding-left: 0;"><img src="../../assets/image/article2.jpg" align="articleList" /></div>
                    <div class="articleCase">
                        <p class="blod">
                            操作规范
                            <span class="red-hint">待审核</span>
                        </p>
                        <p class="oneLine">这是操作规范的简单说明</p>
                        <p>2018-08-21</p>
                        <i class="mint-cell-allow-right"></i>
                    </div>
                </router-link>
            </li>    
            <li class="mui-table-view-cell">
                <router-link :to="{path: '/guide-detail', query: {controlShow}}" class="mui-navigate-right articleList">
                    <div style="width: 60px; height: 60px; padding-left: 0;"><img src="../../assets/image/article3.jpg" align="articleList" /></div>
                    <div class="articleCase">
                        <p class="blod">
                            系统文章
                            <span class="green-hint">已审核</span>
                        </p>
                        <p class="oneLine">系统文章的简单说明</p>
                        <p>2018-08-21</p>
                        <i class="mint-cell-allow-right"></i>
                    </div>
                </router-link>
            </li>
        </ul>
    </div>
</template>

<script>
export default {
    // props: ['articleList']
    data () {
        return {
            controlShow: true
        }
        
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.guide-margin {
    margin-top: 10px;
    .articleList {
        overflow: hidden; 
        display: flex; 
        position: relative;
        img {
            float: left; 
            width: 60px; 
            height: 60px; 
            border-radius: 8px;
        }
        .articleCase {
            flex: 1; 
            padding-left: 10px;
            p {
                color: $color-afont; 
                font-size: $font-m;
            }
            .oneLine {
                width: 230px;
                overflow:hidden;
                text-overflow:ellipsis;
                white-space:nowrap;
            }
            .blod {
                font-size: $font-l;
                .red-hint {
                    float: right; 
                    font-size: $font-m;
                }
                .green-hint {
                    float: right; 
                    font-size: $font-m;
                }
            }
        }
    }
}



</style>